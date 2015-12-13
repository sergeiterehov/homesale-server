using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homesale.App.Controllers
{
    class Agent : Controller
    {
        public XML Me()
        {
            return new XML()
            .add("id", Auth.Agent.id)
            .add("login", Auth.Agent.login)
            .add("name", Auth.Agent.name)
            .add("addres", Auth.Agent.addres)
            .add("phone", Auth.Agent.phone)
            .add("money", Auth.Agent.money)
            ;
        }

        public XML Get(Requests.Agent.Get request)
        {
            var agent = Models.Agent.Find(request.id);

            if(agent != null)
            {
                return new XML()
                .add("id", agent.id)
                .add("name", agent.name)
                .add("addres", agent.addres)
                .add("phone", agent.phone)
                .add("money", agent.money)
                ;
            }
            else
            {
                throw Main.Abort("Агент не найден!");
            }
        }

        public XML GetList()
        {
            var agents = Models.Agent.All().Get().GetList();
            var list = new XML("list");

            foreach(var agent in agents)
            {
                list.append(
                    new XML("agent")
                    .add("id", agent.id)
                    .add("name", agent.name)
                    .add("phone", agent.phone)
                );
            }

            return new XML().append(list);
        }
    }
}
