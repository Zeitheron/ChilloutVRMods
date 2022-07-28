using ABI_RC.Core;
using ABI_RC.Core.Player;
using OscCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChilloutOSC.Extensions;

using SimpleJSON;

namespace ChilloutOSC.OscEndpoints
{
    internal class ListAvatarParameters : IOscEndpoint
    {
        public string GetAddress()
        {
            return "/avatar/parameters_list";
        }

        public void Trigger(OscMessageValues values, string addr)
        {
            CVRAnimatorManager animator = PlayerSetup.Instance?.animatorManager;
            if(animator != null)
            {
                var pars = animator.ListAllOSCParameters();

                JSONNode root = new JSONNode();
                {
                    JSONArray parameters = new JSONArray();

                    foreach (var par in pars)
                    {
                        JSONNode n = new JSONNode();
                        n.Add("name", par.paramName);
                        n.Add("type", par.type.GetName());

                        var ads = "/avatar/parameters/" + par.paramName;

                        JSONNode input = new JSONNode();
                        input.Add("address", ads);
                        input.Add("type", par.type.GetName());
                        n.Add("input", input);

                        JSONNode output = new JSONNode();
                        output.Add("address", ads);
                        output.Add("type", par.type.GetName());
                        n.Add("output", output);

                        parameters.Add(n);
                    }

                    root.Add("parameters", parameters);
                }

                ChilloutOSC.broadcaster.Send("/avatar/parameters_list", root.ToString());
            }
        }
    }
}