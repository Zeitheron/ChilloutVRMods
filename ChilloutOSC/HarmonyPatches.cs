using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABI.CCK.Components;
using ABI_RC.Core;
using ABI_RC.Core.Player;
using ChilloutOSC.OscEndpoints.Broadcasting;
using HarmonyLib;
using UnityEngine;

namespace ChilloutOSC.HarmonyPatches
{
    [HarmonyPatch(typeof(PlayerSetup), "SetupAvatar")]
    class CVRAnimManagerPatch
    {
        static void Postfix(ref PlayerSetup __instance)
        {
            VRCReloadEmulator.Invoke(__instance);
        }
    }
}