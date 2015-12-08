
namespace homesale.Libs.Base
{
    abstract class Application<T> where T : Application<T>, new()
    {
        protected static T _instance = null;
        public static T ME() { return _instance = _instance ?? new T(); }

        public HSP.Query Query;

        abstract public Response Begin();

        public T Init(HSP.Query Query)
        {
            this.Query = Query;

            return THIS;
        }

        public T THIS
        {
            get
            {
                return (T)(dynamic)this;
            }
        }

        static public void Abort()
        {
            throw new ExceptionAbort();
        }
        static public void Abort(string Message)
        {
            throw new ExceptionAbort(0, Message);
        }
        static public void Abort(int Code)
        {
            throw new ExceptionAbort(Code, "");
        }
        static public void Abort(int Code, string Message)
        {
            throw new ExceptionAbort(Code, Message);
        }
        static public void Abort(string Message, int Code)
        {
            throw new ExceptionAbort(Code, Message);
        }

        public class ExceptionAbort : System.Exception
        {
            private int _code;
            public int Code
            {
                get
                {
                    return this._code;
                }
            }

            public ExceptionAbort(int Code = 0, string Message = "Unknown error!") : base(Message)
            {
                this._code = Code;
            }
        }
    }
}
