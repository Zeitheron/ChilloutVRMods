using OscCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChilloutOSC.OscEndpoints
{
    internal class AllEndpoints
    {
        public static List<IOscEndpoint> allEndpoints = new List<IOscEndpoint>()
        {
            new AvatarParameterTyped(),
            new AvatarParameterVRC(),
            new ListAvatarParameters()
        };

        public static void registerAll(OscServer server)
        {
            foreach(var ep in allEndpoints)
            {
                server.TryAddMethod(ep.GetAddress(), ep.Trigger);
            }
        }
    }

    public interface IOscEndpoint
    {
        String GetAddress();

        void Trigger(OscMessageValues values, string addr);
    }
}