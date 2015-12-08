using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homesale.App.Controllers
{
    class Agent : Controller
    {
        public Response Me()
        {
            return new Response(
                new XML("success").append(
                    XML.Get("name", Auth.Agent.name)
                )
            );
        }
    }
}
