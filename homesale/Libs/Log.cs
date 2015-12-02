using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homesale.Libs
{
    class Log
    {
        public enum Types : int { Default = 0, Primary, Success, Warning, Error };

        public delegate void LogEvent(string message, Types Type, object obj);

        private Log() { }

        static private string History = "";
        static public event LogEvent OnMessage;

        static public void Write(object obj, Types Type = Types.Default)
        {
            string message = obj.GetType().Name + ": " + obj.ToString();
            History += message + "\n";

            OnMessage(message, Type, obj);
        }
    }
}
