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

            var agent = Agent.By((selector) => { return selector.
                where("login", "=", Request.login)
                .where("passwd", "=", Request.passwd)
            ; }).Get();

            if (agent != null)
            {
                result = new XML("success").append(
                    new XML("token").xml(Hash.MD5(agent.passwd + "__SALT_321"))
                ).append(
                    new XML("id").xml(agent.id)
                );
            }
            else
            {
                result = new XML("error").xml("User not found!");
            }

            return Request.Response.Write(result);
        }

        public bool Check()
        {
            return true; // TODO
        }
    }
}
