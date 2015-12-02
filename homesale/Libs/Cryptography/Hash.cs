using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;
using homesale.DataBase;

namespace homesale.Libs.Cryptography
{
    class Hash
    {
        static public byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static public string ToHex(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);

            foreach (byte bi in bytes)
            {
                hex.AppendFormat("{0:x2}", bi);
            }
            return hex.ToString();
        }

        static public string MD5(byte[] Bytes)
        {
            return ToHex(System.Security.Cryptography.MD5.Create().ComputeHash(Bytes));
        }
        static public string MD5(string String)
        {
            return MD5(GetBytes(String));
        }
    }
}
