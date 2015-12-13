
using System;
using homesale.App.Controllers;

namespace homesale.App
{
    class Router : Libs.Base.Router
    {
        override protected void Before()
        {
            switch (CallMethod)
            {
                case "Auth.Login": return;
            }

            Auth.Die();
        }

        protected override XML After(XML Result)
        {
            return Result;
        }
    }
}
