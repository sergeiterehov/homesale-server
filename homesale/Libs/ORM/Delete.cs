using System;
using System.Data;
using homesale.DataBase;

namespace homesale.Libs.ORM
{
    class Delete
    {
        private string _top = "";
        private string _from = "";
        private string _where = "";

        private string _boolMode = " AND ";

        public Delete()
        {

        }
        public Delete(string TableName)
        {
            this.from(TableName);
        }

        public override string ToString()
        {
            string result = "DELETE ";

            if (this._top != "") result += String.Format(" TOP {0} ", this._top);
            if (this._from != "") result += String.Format(" FROM {0} ", this._from);
            if (this._where != "") result += String.Format(" WHERE {0} ", this._where);

            return result;
        }

        public Delete cmpAnd()
        {
            this._boolMode = " AND ";
            return this;
        }
        public Delete cmpOr()
        {
            this._boolMode = " OR ";
            return this;
        }

        public Delete top(int Count)
        {
            this._top = Math.Abs(Count).ToString();
            return this;
        }

        public Delete from(Raw FromRaw)
        {
            this._from = FromRaw.ToString();
            return this;
        }
        public Delete from(string TableName)
        {
            this._from = String.Format("[{0}]", TableName);
            return this;
        }
        public Delete from(Select Select, string AsName = "")
        {
            this._from = String.Format("({0}) [{1}]", Select.ToString(), AsName);
            return this;
        }


        public Delete where(Raw WhereRaw)
        {
            if (this._where != "") this._where += this._boolMode;
            this._where += WhereRaw.ToString();
            return this;
        }
        public Delete where(string Field, string Comparator, dynamic Value)
        {
            if (Value.GetType() == typeof(string)) Value = String.Format("'{0}'", Value);
            string raw = String.Format("[{0}] {1} {2}", Field, Comparator, Value.ToString());
            this.where(new Raw(raw));
            return this;
        }



        public void Do()
        {
            DB.ME().QueryDo(this.ToString());
        }

    }
}
