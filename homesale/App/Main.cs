using homesale.App.Controllers;

namespace homesale.App
{
    class Main : Libs.Base.App
    {
        protected static dynamic _instance = null;
        public static dynamic ME() { return _instance = _instance ?? new Main(); }

        public Response Router() // TODO - вынести в отдельный класс >> Route("Auth", Auth) >> Route("Auth", Auth, "LoginMe", "Login")
        {
            switch (this.Query.CallClass)
            {
                case "Auth":
                    switch (this.Query.CallMethod)
                    {
                        case "Login":
                            return new Auth().Login(new Requests.Auth.Login());
                    }
                    break;
            }
            return null;
        }
    }
}
