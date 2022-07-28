using ABI_RC.Core;
using ABI_RC.Core.EventSystem;
using ABI_RC.Core.Player;
using UnityEngine;
using MelonLoader;
using OscCore;
using System.Text.RegularExpressions;
using System.Collections.Generic;

using ChilloutOSC.Extensions;
using ChilloutOSC.OscEndpoints;

using HarPatch = HarmonyLib.Harmony;
using ABI_RC.Core.InteractionSystem;
using System;

[assembly: MelonInfo(typeof(ChilloutOSC.ChilloutOSC), "ChilloutOSC", "1.0.1", "Zeitheron")]
[assembly: MelonGame("Alpha Blend Interactive", "ChilloutVR")]
namespace ChilloutOSC
{
    public class ChilloutOSC : MelonMod
    {
        internal static HarPatch harmony;
        public static Queue<OSCQueuedMsg> queue = new Queue<OSCQueuedMsg>();

        public static OscServer oscServer { get; private set; }
        public static OscClient broadcaster { get; private set; }

        public override void OnApplicationStart()
        {
            harmony = new HarPatch("org.zeith.CVR.OSC");
            harmony.PatchAll(System.Reflection.Assembly.GetExecutingAssembly());

            oscServer = new OscServer(9000);
            oscServer.Start();

            broadcaster = new OscClient("255.255.255.255", 9001);

            AllEndpoints.registerAll(oscServer);
            MelonLogger.Msg("OSC Server has started on port 9000.");
        }

        private bool needsUpdate;
        private long lastUpdate;

        public override void OnUpdate()
        {
            oscServer.Update();

            // This can help me figure out how to set avi params.
            // PlayerSetup.Instance.animatorManager.

            var player = PlayerSetup.Instance;

            // player.GetAvatarDescriptor().avatarSettings.baseController;

            CVRAnimatorManager animator = PlayerSetup.Instance?.animatorManager;
            var menu = CVR_MenuManager.Instance;

            while (queue.Count > 0)
            {
                var msg = queue.Dequeue();

                switch (msg.type)
                {
                    case OSCMsgType.INT:
                        if (animator != null) animator.SetAnimatorParameterInt(msg.paramName, msg.paramInt);
                        break;
                    case OSCMsgType.BOOL:
                        if (animator != null) animator.SetAnimatorParameterBool(msg.paramName, msg.paramBool);
                        break;
                    case OSCMsgType.FLOAT:
                        if (animator != null) animator.SetAnimatorParameterFloat(msg.paramName, msg.paramFloat);
                        if (menu != null) menu.SendAdvancedAvatarUpdate(msg.paramName, msg.paramInt, true);
                        break;
                }

                needsUpdate = true;
            }

            long now;
            if (needsUpdate && lastUpdate - (now = DateTime.UtcNow.currentTimeMillis()) > 100)
            {
                lastUpdate = now;
                needsUpdate = false;
                ViewManager.Instance.RequestCurrentAvatarSettings();
            }
        }

        public override void OnApplicationQuit()
        {
            if (oscServer != null)
            {
                oscServer.Dispose();
                oscServer = null;
            }
        }
    }

    public enum OSCMsgType
    {
        INT,
        BOOL,
        FLOAT
    }

    public struct OSCQueuedMsg
    {
        public OSCMsgType type;
        public string paramName;

        public bool paramBool;
        public int paramInt;
        public float paramFloat;
    }
}