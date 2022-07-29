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
using System.IO;

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

                JSONClass root = new JSONClass();
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

                string json = root.ToString();

                ChilloutOSC.broadcaster.Send("/avatar/parameters_list", json);
            }
        }
    }
}