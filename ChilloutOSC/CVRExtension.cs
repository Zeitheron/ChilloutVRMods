using ABI.CCK.Components;
using ABI_RC.Core;
using ABI_RC.Core.Player;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChilloutOSC.Extensions
{
    public static class CExtensions
    {
        static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static long currentTimeMillis(this DateTime time)
        {
            return (long) (time - epoch).TotalMilliseconds;
        }
    }

    public static class CVRExtension
    {
        public static CVRAvatar GetAvatarDescriptor(this PlayerSetup setup)
        {
            return setup._avatar.GetComponent<CVRAvatar>();
        }

        public static string GetAvatarGuid(this PlayerSetup setup)
        {
            return setup._avatar.GetComponent<CVRAssetInfo>().guid;
        }

        public static List<OSCQueuedMsg> ListAllOSCParameters(this CVRAnimatorManager manager)
        {
            List<OSCQueuedMsg> paramList = new List<OSCQueuedMsg>();

            Animator _animator = Traverse.Create(manager).Field("_animator").GetValue<Animator>();

            for (int index = 0; index < _animator.parameterCount; ++index)
            {
                AnimatorControllerParameter parameter = _animator.parameters[index];

                var type = parameter.type;

                if (type == AnimatorControllerParameterType.Trigger) continue;

                OSCQueuedMsg msg;

                msg.paramName = parameter.name;
                msg.paramBool = parameter.defaultBool;
                msg.paramInt = parameter.defaultInt;
                msg.paramFloat = parameter.defaultFloat;

                switch(type)
                {
                    case AnimatorControllerParameterType.Float:
                        msg.paramFloat = _animator.GetFloat(parameter.nameHash);
                        msg.type = OSCMsgType.FLOAT;
                        break;
                    case AnimatorControllerParameterType.Int:
                        msg.paramInt = _animator.GetInteger(parameter.nameHash);
                        msg.type = OSCMsgType.FLOAT;
                        break;
                    case AnimatorControllerParameterType.Bool:
                        msg.paramBool = _animator.GetBool(parameter.nameHash);
                        msg.type = OSCMsgType.FLOAT;
                        break;
                    default: // unsupported type found!
                        continue;
                }

                paramList.Add(msg);
            }

            return paramList;
        }
    }

    public static class EnumOSCMsgTypeExt
    {
        public static string GetName(this OSCMsgType type)
        {
            switch (type)
            {
                case OSCMsgType.BOOL:
                    return "bool";
                case OSCMsgType.INT:
                    return "int";
                case OSCMsgType.FLOAT:
                    return "float";
            }
            return "unknown";
        }
        public static string GetNameC(this OSCMsgType type)
        {
            switch (type)
            {
                case OSCMsgType.BOOL:
                    return "Bool";
                case OSCMsgType.INT:
                    return "Int";
                case OSCMsgType.FLOAT:
                    return "Float";
            }
            return "unknown";
        }

        public static OSCMsgType ToOSCMsgType(this string type)
        {
            type = type.ToLower();

            switch (type)
            {
                case "bool":
                    return OSCMsgType.BOOL;
                case "int":
                    return OSCMsgType.INT;
                case "float":
                    return OSCMsgType.FLOAT;
            }

            return OSCMsgType.BOOL;
        }
    }
}
