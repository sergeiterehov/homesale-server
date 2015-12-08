using System;
using System.Data;
using homesale.DataBase;

namespace homesale.Libs.ORM
{
    class Insert
    {
        private string _top = "";
        private string _into = "";
        private string _columns = "";
        private string _from = "";
        private string _values = "";

        public override string ToString()
        {
            string result = "INSERT ";

            if (this._top != "") result += String.Format(" TOP {0} ", this._top);
            if (this._into != "") result += String.Format(" INTO {0} ", this._into);
            if (this._columns != "") result += String.Format(" ({0}) ", this._columns);
            result += " OUTPUT INSERTED.ID ";
            if (this._from != "") result += String.Format(" {0} ", this._from);
            if (this._values != "") result += String.Format(" VALUES {0} ", this._values);

            return result;
        }

        public Insert()
        {

        }
        public Insert(string TableName)
        {
            this.into(TableName);
        }

        public Insert top(int Count)
        {
            this._top = Math.Abs(Count).ToString();
            return this;
        }

        public Insert into(Raw FromRaw)
        {
            this._into = FromRaw.ToString();
            return this;
        }
        public Insert into(string TableName)
        {
            this._into = String.Format("[{0}]", TableName);
            return this;
        }

        public Insert column(Raw SelectRaw)
        {
            if (this._columns != "") this._columns += ", ";
            this._columns += SelectRaw.ToString();
            return this;
        }
        public Insert column(string Field)
        {
            this.column(new string[] { Field });
            return this;
        }
        public Insert column(params dynamic[] Fields)
        {
            string raw = "";
            foreach (string field in Fields)
            {
                if (raw != "") raw += ", ";
                raw += String.Format("[{0}]", field.ToString());
            }
            this.column(new Raw(raw));
            return this;
        }

        public Insert from(Raw FromRaw)
        {
            this._from = FromRaw.ToString();
            return this;
        }
        public Insert from(Select SelectTable)
        {
            this.from(new Raw(SelectTable.ToString()));
            return this;
        }

        public Insert value(Raw ValueRaw)
        {
            if (this._values != "") this._values += ", ";
            this._values += ValueRaw.ToString();
            return this;
        }
        public Insert value(dynamic [] Fields)
        {
            string raw = "";
            foreach(dynamic field in Fields)
            {
                if (raw != "") raw += ", ";
                string value = field.ToString();
                if (field.GetType() == typeof(string)) value = String.Format("'{0}'", value);
                raw += String.Format("{0}", value);

                this.value(new Raw(raw));
            }
            return this;
        }



        public long Do()
        {
            return DB.ME().QueryDo<long>(this.ToString());
        }

    }
}
