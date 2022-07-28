using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ABI.CCK.Components;
using ABI_RC.Core;
using ABI_RC.Core.Player;
using HarmonyLib;
using UnityEngine;

namespace ChilloutOSC.HarmonyPatches
{
    [HarmonyPatch(typeof(PlayerSetup), "PreSetupAvatarGeneral")]
    class CVRAnimManagerPatch
    {
        static void Postfix(ref CVRAvatar ____avatarDescriptor)
        {
            
        }
    }
}