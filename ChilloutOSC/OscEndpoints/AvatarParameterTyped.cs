using MelonLoader;
using OscCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChilloutOSC.Extensions;

namespace ChilloutOSC.OscEndpoints
{
    internal class AvatarParameterTyped : IOscEndpoint
    {
        public const string OSC_AVTR_PARAM_REGEX = @"\/avatar\/parameters\/(?<type>(float)|(int)|(bool))\/(?<param_name>.+)";

        public string GetAddress()
        {
            return "/avatar/parameters_(?<type>(float)|(int)|(bool))/(?<param_name>.+)";
        }

        public void Trigger(OscMessageValues values, string addr)
        {
            Match m = Regex.Matches(addr, OSC_AVTR_PARAM_REGEX)[0];

            if (m.Success)
            {
                string type = m.Groups["type"].Value;
                string param_name = m.Groups["param_name"].Value;

                OSCQueuedMsg msg;
                msg.type = type.ToOSCMsgType();
                msg.paramName = param_name;

                msg.paramBool = false;
                msg.paramInt = 0;
                msg.paramFloat = 0;

                if (msg.type == OSCMsgType.INT)
                {
                    msg.paramInt = values.ReadIntElement(0);
                    MelonLogger.Msg("Got " + msg.paramInt + " int element on " + addr);
                }

                if (msg.type == OSCMsgType.FLOAT)
                {
                    msg.paramFloat = values.ReadFloatElement(0);
                    MelonLogger.Msg("Got " + msg.paramFloat + " float element on " + addr);
                }

                if (msg.type == OSCMsgType.BOOL)
                {
                    msg.paramBool = values.ReadBooleanElement(0);
                    MelonLogger.Msg("Got " + msg.paramBool + " bool element on " + addr);
                }

                ChilloutOSC.queue.Enqueue(msg);
            }
        }
    }
}
