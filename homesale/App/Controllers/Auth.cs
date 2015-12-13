using homesale.Libs.ORM;
using homesale.Libs.Cryptography;
using homesale.App.Models;

namespace homesale.App.Controllers
{
    class Auth : Controller
    {
        public XML Login(Requests.Auth.Login request)
        {
            request.passwd = Hash.MD5(request.passwd + "__SALT_123");

            var agent = Models.Agent.By((selector) => { return selector.
                where("login", "=", request.login)
                .where("passwd", "=", request.passwd)
            ; }).Get();

            if (agent != null)
            {
                return new XML()
                .add("token", Hash.MD5(agent.passwd + "__SALT_321"))
                .add("tokenId", agent.id)
                ;
            }
            else
            {
                throw Main.Abort("Не верно введен логин или пароль!");
            }
        }

        static private Models.Agent _agent = null;

        static public Models.Agent Agent
        {
            get
            {
                return _agent;
            }
        }

        static public void AuthToken()
        {
            if (Request.has("token") && Request.has("tokenId"))
            {
                var token = Request.input<string>("token");
                var id = Request.input<long>("tokenId");

                var agent = Models.Agent.Find(id);

                if(agent != null && token == Hash.MD5(agent.passwd + "__SALT_321"))
                {
                    _agent = agent;
                }
                else
                {
                    _agent = null;
                }
            }
        }

        static public bool Check
        {
            get
            {
                return _agent != null;
            }
        }

        static public void Die()
        {
            if (!Check) throw Main.Abort(1, "Требуется аутентификация!");
        }
    }
}
