using MelonLoader;
using OscCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ChilloutOSC.OscEndpoints
{
    internal class AvatarParameterVRC : IOscEndpoint
    {
        public const string OSC_AVTR_PARAM_REGEX_TAGGED = @"\/avatar\/parameters\/(?<param_name>.+)";

        public string GetAddress()
        {
            return "/avatar/parameters/(?<param_name>.+)";
        }

        public void Trigger(OscMessageValues values, string addr)
        {
            Match m = Regex.Matches(addr, OSC_AVTR_PARAM_REGEX_TAGGED)[0];

            if (m.Success)
            {
                string param_name = m.Groups["param_name"].Value;

                var tagType = values.Tags[0];

                OSCQueuedMsg msg;

                msg.type = (tagType == TypeTag.True || tagType == TypeTag.False) ? OSCMsgType.BOOL
                    : (tagType == TypeTag.Float32 || tagType == TypeTag.Float64) ? OSCMsgType.FLOAT
                    : (tagType == TypeTag.Int32 || tagType == TypeTag.Int64) ? OSCMsgType.INT
                    : OSCMsgType.BOOL;

                msg.paramName = param_name;

                msg.paramBool = false;
                msg.paramInt = 0;
                msg.paramFloat = 0;

                switch(msg.type)
                {
                    case OSCMsgType.INT:
                        msg.paramInt = values.ReadIntElement(0);
                        MelonLogger.Msg("Got " + msg.paramInt + " int element on " + addr);
                        break;

                    case OSCMsgType.FLOAT:
                        msg.paramFloat = values.ReadFloatElement(0);
                        MelonLogger.Msg("Got " + msg.paramFloat + " float element on " + addr);
                        break;

                    case OSCMsgType.BOOL:
                        msg.paramBool = values.ReadBooleanElement(0);
                        MelonLogger.Msg("Got " + msg.paramBool + " bool element on " + addr);
                        break;
                }

                ChilloutOSC.queue.Enqueue(msg);
            }
        }
    }
}
