using homesale.Libs.ORM;
using homesale.Libs.Cryptography;
using homesale.App.Models;

namespace homesale.App.Controllers
{
    class Auth : Controller
    {
        public Response Login(Requests.Auth.Login Request)
        {
            XML result;

            Request.passwd = Hash.MD5(Request.passwd + "__SALT_123");

            var agent = Models.Agent.By((selector) => { return selector.
                where("login", "=", Request.login)
                .where("passwd", "=", Request.passwd)
            ; }).Get();

            if (agent != null)
            {
                result = new XML("success").append(
                    new XML("token").xml(Hash.MD5(agent.passwd + "__SALT_321"))
                ).append(
                    new XML("token-id").xml(agent.id)
                );
            }
            else
            {
                result = new XML("error").xml("User not found!");
            }

            return Request.Response.Write(result);
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
            if (Request.has("token") && Request.has("token-id"))
            {
                var token = Request.input<string>("token");
                var id = Request.input<long>("token-id");

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
            if (!Check) Main.Abort(1, "Требуется аутентификация!");
        }
    }
}
