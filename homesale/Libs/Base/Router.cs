using System;
using System.Collections.Generic;


namespace homesale.Libs.Base
{
    abstract class Router
    {
        protected string ControllersNamespace;
        protected string Class;
        protected string Method;
        protected HSP.Query Query;

        protected string CallClass;
        protected string CallClassFull;

        protected string CallMethod;
        protected string CallMethodFull;

        abstract protected void Before();
        abstract protected XML After(XML Result);

        public XML Call(string ControllersNamespace, string CallClass, string CallMethod, HSP.Query Query)
        {
            this.ControllersNamespace = ControllersNamespace;
            this.Class = CallClass;
            this.Method = CallMethod;
            this.Query = Query;

            this.CallClass = CallClass;
            this.CallClassFull = this.ControllersNamespace + "." + this.CallClass;

            this.CallMethod = this.CallClass + "." + CallMethod;
            this.CallMethodFull = this.CallClassFull + "." + this.CallMethod;

            this.Before();

            return this.After((XML)CallController());
        }

        private object CallController()
        {
            var callType = Type.GetType(this.CallClassFull);

            if (callType == null)
            {
                throw new ExceptionRouteNotFound(string.Format("Класс \"{0}\" не найден!", this.CallClass));
            }

            var callObject = callType.GetConstructor(new Type[] { }).Invoke(new object[] { });
            var callMethod = callType.GetMethod(this.Method);

            if(callMethod == null)
            {
                throw new ExceptionRouteNotFound(string.Format("Метод \"{0}\" не найден!", this.CallMethod));
            }

            var parameters = callMethod.GetParameters();
            List<object> args = new List<object>();

            foreach (var param in parameters)
            {
                args.Add(Activator.CreateInstance(param.ParameterType));
            }

            return callMethod.Invoke(callObject, args.ToArray());
        }

        //------------ EXCEPTIONS

        public class ExceptionRouteNotFound : Exception
        {
            public ExceptionRouteNotFound(string Message = "Класс или метод запроса не найден!") : base(Message)
            {

            }
        }
    }
}
