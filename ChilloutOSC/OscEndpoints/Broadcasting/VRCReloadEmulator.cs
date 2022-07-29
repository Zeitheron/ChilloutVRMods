using ABI.CCK.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChilloutOSC.Extensions;
using ABI_RC.Core;
using ABI_RC.Core.Player;
using SimpleJSON;
using MelonLoader;
using ABI_RC.Core.Savior;

namespace ChilloutOSC.OscEndpoints.Broadcasting
{
    internal class VRCReloadEmulator
    {
        public static readonly string VRCData = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)
                            .Replace("Roaming", "LocalLow"),
                "VRChat",
                "VRChat"
        );

        public static readonly string USER = "usr_ChilloutVR_OSC";

        public static readonly string VRCAvatarsOSCDirectory = Path.Combine(VRCData, "OSC", USER, "Avatars");

        private static string lastAvtrId = null;

        public static void Invoke(PlayerSetup player)
        {
            if (!Directory.Exists(VRCAvatarsOSCDirectory))
                Directory.CreateDirectory(VRCAvatarsOSCDirectory);

            var name = player._avatar.name;
            var pars = player.animatorManager.ListAllOSCParameters();

            var avtrIdCur = MetaPort.Instance?.currentAvatarGuid;
            var avtrIdFromSetup = player?.GetAvatarGuid();

            var avtrId = "avtr_" + (avtrIdCur != null ? avtrIdCur : avtrIdFromSetup);

            if (lastAvtrId == avtrId) return;

            lastAvtrId = avtrId;

            var avtrFilePath = Path.Combine(VRCAvatarsOSCDirectory, avtrId + ".json");

            MelonLogger.Msg("Broadcasting OSC reload avatar " + avtrId);

            JSONClass root = new JSONClass();
            root["id"] = avtrId;
            root["name"] = name;
            {
                JSONArray parameters = new JSONArray();

                foreach (var par in pars)
                {
                    JSONClass n = new JSONClass();
                    n["name"] = par.paramName;
                    n["type"] = par.type.GetNameC();
                    n["input"]["type"] = n["output"]["type"] = par.type.GetNameC();
                    n["input"]["address"] = n["output"]["address"] = "/avatar/parameters/" + par.paramName;
                    parameters.Add(n);
                }
                root["parameters"] = parameters;
            }

            File.WriteAllText(avtrFilePath, root.ToString(""));

            ChilloutOSC.broadcaster.Send("/avatar/change", avtrId);
        }
    }
}