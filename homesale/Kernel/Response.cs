using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace homesale
{
    class Response
    {
        private MemoryStream Memmory = new MemoryStream();

        public Response()
        {
            
        }
        public Response(params dynamic[] Objects)
        {
            foreach (dynamic obj in Objects)
            {
                this.Write(obj.ToString());
            }
        }

        public Response Write(byte[] Buffer)
        {
            this.Memmory.Write(Buffer, 0, Buffer.Length);

            return this;
        }
        public Response Write(string String)
        {
            byte[] Buffer = System.Text.Encoding.UTF8.GetBytes(String);
            this.Write(Buffer);

            return this;
        }
        public Response Write(params dynamic[] Objects)
        {
            foreach(dynamic obj in Objects)
            {
                this.Write(obj.ToString());
            }
            return this;
        }
        public Response Write(Response Response)
        {
            if(Response != null)
            {
                this.Write(Response.GetBytes());
            }

            return this;
        }

        public byte[] GetBytes()
        {
            long tmpPosition = this.Memmory.Position;
            this.Memmory.Position = 0;
            byte[] Buffer = new byte[this.Memmory.Length];
            this.Memmory.Read(Buffer, 0, Buffer.Length);
            this.Memmory.Position = tmpPosition;

            return Buffer;
        }

        public string GetString()
        {
            return System.Text.Encoding.UTF8.GetString(this.GetBytes());
        }
    }
}
