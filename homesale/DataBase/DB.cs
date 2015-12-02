using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using homesale.Libs;

namespace homesale.DataBase
{
    class DB
    {
        static private DB INSTANCE = null;
        static public DB ME(){return null == DB.INSTANCE ? DB.INSTANCE = new DB() : DB.INSTANCE;}

        private SqlConnection Connection;

        private DB()
        {

        }

        public SqlConnection c()
        {
            return this.Connection;
        }

        public DB Connect(string Server, string Catalog, string UserName = null, string UserPassword = null)
        {
            Log.Write("Формирование строки подключения...");

            string ConnectionStrign = String.Format("Data Source={0};Initial Catalog={1};", Server, Catalog);
            
            if(null == UserName)
            {
                ConnectionStrign += String.Format("Integrated Security=SSPI;User ID = {0};Password = {1};", UserName, UserPassword);
            }
            else
            {
                ConnectionStrign += "Trusted_connection=Yes;";
            }


            Log.Write("Подключение...");
            try
            {
                this.Connection = new SqlConnection(ConnectionStrign);
                this.Connection.Open();

                Log.Write("Статус соединения: " + this.Connection.State.ToString(), Log.Types.Primary);
            }
            catch (Exception ex)
            {
                throw new Exception("Ошибка при попытке создать подключение. " + ex.Message);
            }

            return this;
        }

        public List<Field> GetTableFields(string TableName)
        {
            List<Field> result = new List<Field>();

            try
            {
                SqlCommand sc = this.Connection.CreateCommand();
                sc.CommandText = String.Format("select * from [{0}];", TableName);
                SqlDataReader sr = sc.ExecuteReader();
                sr.Read();

                for(int i=0; i<sr.FieldCount; i++)
                {
                    result.Add(new Field() { Name = sr.GetName(i), Type = sr.GetFieldType(i) });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Не удалось получить поля таблицы " + TableName + ". " + ex.Message);
            }
            

            return result;
        }

        public DataTable Query(string QueryString)
        {
            DataTable result = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter(QueryString, this.Connection);
            da.FillSchema(result, SchemaType.Source);
            da.Fill(result);

            return result;
        }

        public DB QueryDo(string QueryString)
        {
            SqlCommand sc = this.Connection.CreateCommand();
            sc.CommandText = QueryString;
            sc.ExecuteNonQuery();

            return this;
        }


        public DB Insert(string TableName, string[] Fields, object[] Values)
        {
            string fieldsString = "";
            string valueString = "";

            for(int i=0; i< Fields.Length; i++)
            {
                string field = Fields[i];
                object value = Values[i];
                
                fieldsString += (i > 0 ? "," : "") + "["+ field +"]";
                valueString += (i > 0 ? "," : "") + "'" + (value ?? "").ToString().Replace("'", "\'") + "'";
            }

            this.QueryDo(String.Format("insert into [{0}]({1})values({2});", TableName, fieldsString, valueString));

            return this;
        }

        public DB Update(string TableName, string PrimaryName, string PrimaryOperator, object PrimaryValue, string[] Fields, object[] Values)
        {
            string valueString = "";

            for (int i = 0; i < Fields.Length; i++)
            {
                string field = Fields[i];
                object value = Values[i];

                valueString += (i > 0 ? "," : "") + "[" + field + "]=" + "'" + value.ToString().Replace("'", "\'") + "'";
            }

            this.QueryDo(String.Format("update [{0}] set {1} where [{2}]{3}'{4}';", TableName, valueString, PrimaryName, PrimaryOperator, PrimaryValue));

            return this;
        }

        static public string Str(string Source)
        {
            return Source.Replace("'", "\'");
        }


        //-----------------> CLASS

        public struct Field
        {
            public string Name;
            public Type Type;
        }
    }
}
