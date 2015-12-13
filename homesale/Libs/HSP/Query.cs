using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.IO;

namespace homesale.Libs.HSP
{
    class Query
    {
        private TcpClient Me;
        private NetworkStream Stream;

        public string Body;

        public XDocument XML;

        public string CallClass, CallMethod;

        public Query(TcpClient Client)
        {
            this.Me = Client;

            Stream = Client.GetStream();
            string Request = "";
            byte[] Buffer = new byte[4096];
            int Count = 0;

            while ((Count = Stream.Read(Buffer, 0, Buffer.Length)) > 0)
            {
                Request += Encoding.UTF8.GetString(Buffer, 0, Count);

                if (Buffer.Length > Count)
                {
                    break;
                }
            }

            this.Body = Request;

            try
            {
                this.XML = XDocument.Parse(this.Body);
                

                this.CallClass = XML.Element(XName.Get("call")).Attribute(XName.Get("class")).Value;
                this.CallMethod = XML.Element(XName.Get("call")).Attribute(XName.Get("method")).Value;
            }
            catch(Exception ex)
            {
                throw new Exception("Ошибка в формате запроса. " + ex.Message);
            }
        }

        public void Write(byte[] Buffer)
        {
            this.Stream.Write(Buffer, 0, Buffer.Length);
        }

        public void Write(string String)
        {
            this.Write(Encoding.UTF8.GetBytes(String));
        }
        public void Write(params string[] Strings)
        {
            foreach(string str in Strings)
            {
                this.Write(str);
            }
        }
    }
}
