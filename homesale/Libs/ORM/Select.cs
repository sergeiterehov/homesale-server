using System;
using System.Data;
using homesale.DataBase;

namespace homesale.Libs.ORM
{
    class Select
    {
        private string _select = "";
        private string _top = "";
        private string _into = "";
        private string _from = "";
        private string _where = "";
        private string _groupBy = "";
        private string _having = "";
        private string _orderBy = "";

        private string _boolMode = " AND ";

        public Select()
        {

        }
        public Select(Raw FromRaw)
        {
            this.from(FromRaw);
        }
        public Select(string TableName)
        {
            this.from(TableName);
        }
        public Select(Select Select, string AsName = "")
        {
            this.from(Select);
        }

        public override string ToString()
        {
            string result = "SELECT ";

            if (this._top != "") result += String.Format(" TOP {0} ", this._top);
            result += String.Format(" {0} ", this._select == "" ? "*" : this._select);
            if (this._into != "") result += String.Format(" INTO {0} ", this._into);
            if (this._from != "") result += String.Format(" FROM {0} ", this._from);
            if (this._where != "") result += String.Format(" WHERE {0} ", this._where);
            if (this._groupBy != "") result += String.Format(" GROUP BY {0} ", this._groupBy);
            if (this._having != "") result += String.Format(" HAVING {0} ", this._having);
            if (this._orderBy != "") result += String.Format(" ORDER BY {0} ", this._orderBy);

            return result;
        }


        public Select cmpAnd()
        {
            this._boolMode = " AND ";
            return this;
        }
        public Select cmpOr()
        {
            this._boolMode = " OR ";
            return this;
        }


        public Select select(Raw SelectRaw)
        {
            if (this._select != "") this._select += ", ";
            this._select += SelectRaw.ToString();
            return this;
        }
        public Select select(string Field)
        {
            this.select(new string[]{Field});
            return this;
        }
        public Select select(Select Select, string AsName = "")
        {
            string raw = String.Format("({0})", Select);
            if (AsName != "") raw += String.Format(" as [{0}] ", AsName);
            this.select(new Raw(raw));
            return this;
        }
        public Select select(params dynamic[] Fields)
        {
            string raw = "";
            foreach (string field in Fields)
            {
                if (raw != "") raw += ", ";
                raw += String.Format("[{0}]", field.ToString());
            }
            this.select(new Raw(raw));
            return this;
        }


        public Select name(string SelectAsName)
        {
            this._select += String.Format(" as [{0}] ", SelectAsName);
            return this;
        }


        public Select top(int Count)
        {
            this._top = Math.Abs(Count).ToString();
            return this;
        }


        public Select from(Raw FromRaw)
        {
            this._from = FromRaw.ToString();
            return this;
        }
        public Select from(string TableName)
        {
            this._from = String.Format("[{0}]", TableName);
            return this;
        }
        public Select from(Select Select, string AsName = "")
        {
            this._from = String.Format("({0}) [{1}]", Select.ToString(), AsName);
            return this;
        }


        public Select where(Raw WhereRaw)
        {
            if (this._where != "") this._where += this._boolMode;
            this._where += WhereRaw.ToString();
            return this;
        }
        public Select where(string Field, string Comparator, dynamic Value)
        {
            if (Value.GetType() == typeof(string)) Value = String.Format("'{0}'", Value);
            string raw = String.Format("[{0}] {1} {2}", Field, Comparator, Value.ToString());
            this.where(new Raw(raw));
            return this;
        }

        
        public Select order(Raw OrderRaw)
        {
            if (this._orderBy != "") this._orderBy += ", ";
            this._orderBy += OrderRaw.ToString();
            return this;
        }
        public Select order(string Field, bool Desc = false)
        {
            string raw = String.Format("[{0}] {1}", Field, Desc ? "DESC" : "ASC");
            this.order(new Raw(raw));
            return this;
        }

        

        public DataTable Get()
        {
            return DB.ME().Query(this.ToString());
        }
    }
}
