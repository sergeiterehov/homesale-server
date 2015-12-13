using System;
using System.Collections.Generic;using System.Linq;
using System.Text;
using System.Threading.Tasks;

using homesale.Libs;
using homesale.Libs.HSP;

using homesale.DataBase;

namespace homesale
{
    class Program
    {
        static private DB DataBase;
        static private Server server;

        static void LogWrite(string message, Log.Types type, object obj)
        {
            var LastColor = Console.ForegroundColor;
            Console.ForegroundColor =
                type == Log.Types.Primary ? ConsoleColor.Cyan :
                type == Log.Types.Success ? ConsoleColor.Green : 
                type == Log.Types.Warning ? ConsoleColor.Yellow :
                type == Log.Types.Error ? ConsoleColor.Red :
                ConsoleColor.White;
            Console.WriteLine(message);
            Console.ForegroundColor = LastColor;
        }

        static void Main(string[] args)
        {
            Log.OnMessage += LogWrite;

            try
            {
                Init();
                Log.Write("Программа завершена!", Log.Types.Success);
            }
            catch (Exception ex)
            {
                Log.Write("ОШИБКА: " + ex.Message, Log.Types.Error);
            }
        }

        static void Init()
        {
            Log.Write("Инициализация подключения...");
            try
            {
                InitDataBase();
                Log.Write("Подключено!", Log.Types.Success);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка подключения. " + ex.Message);
            }

            Log.Write("Старт сервера...");
            try
            {
                InitInterface();
                Log.Write("Интерфейс остановлен!", Log.Types.Primary);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка интерфейса. " + ex.Message);
            }
            
        }

        static void InitDataBase()
        {
            DataBase = DB.ME();
            DataBase.Connect("VM-WIN8X64", "realtor", "application", "12345"); //VM-WIN8X64
        }

        static void InitInterface()
        {
            server = new Server(14222, Query);
        }

        static public void Query(Query Query)
        {
            Log.Write("QUERY:" + Query.XML);

            Response Response = App.Main.ME().Init(Query).Begin();
            if (null != Response)
            {
                Query.Write(Response.GetBytes());
            }
        }
    }
}
