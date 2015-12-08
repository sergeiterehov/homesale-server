using System;

using homesale.App.Controllers;
using homesale.App.Requests;

namespace homesale.App
{
    static class Router
    {
        public static Response Route(string Class, string Method, Libs.HSP.Query Query)
        {
            switch (Class)
            {
                case "Auth":
                    switch (Method)
                    {
                        case "Login":
                            return new Auth().Login(new Requests.Auth.Login());
                    }
                    break;
                case "Agent":
                    switch (Method)
                    {
                        case "Me": Auth.Die();
                            return new Agent().Me();
                    }
                    break;
            }
            
            return null;
        }
    }
}
