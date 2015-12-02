using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;

namespace homesale.Libs.HSP
{
    class Query
    {
        private TcpClient Me;
        private NetworkStream Stream;

        public string Method, URL, Version, Body;
        public List<string> Headers;

        public XmlReader XML;

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

            Match ReqMatch = Regex.Match(Request, @"^(?<method>\w*)\s(?<url>\S*)\sHTTP\/(?<version>\S*)\r\n(?<headers>.*)\r\n\r\n(?<body>.*)$", RegexOptions.Singleline);

            this.Headers = new List<string>();
            foreach (Capture cap in Regex.Match(ReqMatch.Groups[4].Value, @"^((?<header>.*)\r\n)*").Groups[1].Captures)
            {
                this.Headers.Add(cap.Value);
            }
            this.Method = ReqMatch.Groups[1].Value;
            this.URL = ReqMatch.Groups[2].Value;
            this.Version = ReqMatch.Groups[3].Value;
            this.Body = ReqMatch.Groups[5].Value;

            try
            {
                this.XML = XmlReader.Create(new StringReader(this.Body));

                XML.ReadToFollowing("call");
                XML.MoveToAttribute("class");
                this.CallClass = XML.Value;
                XML.MoveToAttribute("method");
                this.CallMethod = XML.Value;
            }
            catch
            {

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
