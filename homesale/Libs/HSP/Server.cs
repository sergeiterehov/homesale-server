using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace homesale.Libs.HSP
{
    class Server
    {
        TcpListener Listener;

        public Server(int Port, ClientEvent CallBack)
        {
            this.OnQuery += CallBack;

            this.Listener = new TcpListener(IPAddress.Any, Port);
            this.Listener.Start();

            Log.Write("Сервер запущен!", Log.Types.Success);

            while (true)
            {
                try
                {
                    TcpClient Client = Listener.AcceptTcpClient();

                    this.OnQuery(new Query(Client));

                    Client.GetStream().Close();
                }
                catch(Exception ex)
                {
                    Log.Write("Ошибка при обработке запроса. " + ex.Message, Log.Types.Warning);
                }
            }
        }

        ~Server()
        {
            this.Listener.Stop();
        }

        public delegate void ClientEvent(Query Query);
        public event ClientEvent OnQuery;
    }
}
