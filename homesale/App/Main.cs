using System;
using homesale.App.Controllers;

namespace homesale.App
{
    class Main : Libs.Base.Application<Main>
    {
        public override Response Begin()
        {
            try
            {
                Auth.AuthToken();

                XML result = new Router().Call(
                    typeof(Controller).Namespace,
                    this.Query.CallClass,
                    this.Query.CallMethod,
                    this.Query
                );

                if (result == null) result = new XML();

                return new Response(result.name("success"));

            }
            catch (ExceptionAbort ex)
            {
                return new Response(new XML("error").append(
                    XML.Get("code", ex.Code)
                ).append(
                    XML.Get("message", ex.Message)
                ));
            }
            catch (Exception ex)
            {
                return new Response(new XML("error").append(
                    XML.Get("code", -1)
                ).append(
                    XML.Get("message", ex.Message)
                ));
            }
        }
    }
}
