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

                Response result = Router.Route(this.Query.CallClass, this.Query.CallMethod, this.Query);

                if (result == null) Main.Abort("Не найден \""+ this.Query.CallClass + "." + this.Query.CallMethod +"\"!");

                return result;

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
