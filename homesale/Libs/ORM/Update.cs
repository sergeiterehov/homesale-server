﻿using System;
using System.Data;
using homesale.DataBase;

namespace homesale.Libs.ORM
{
    class Update
    {
        private string _top = "";
        private string _table = "";
        private string _set = "";
        private string _from = "";
        private string _where = "";

        private string _boolMode = " AND ";

        public Update()
        {

        }
        public Update(string TableName)
        {
            this.table(TableName);
        }

        public override string ToString()
        {
            string result = "UPDATE ";

            if (this._top != "") result += String.Format(" TOP {0} ", this._top);
            if (this._table != "") result += String.Format(" {0} ", this._table);
            if (this._set != "") result += String.Format(" SET {0} ", this._set);
            if (this._from != "") result += String.Format(" FROM {0} ", this._from);
            if (this._where != "") result += String.Format(" WHERE {0} ", this._where);

            return result;
        }

        public Update cmpAnd()
        {
            this._boolMode = " AND ";
            return this;
        }
        public Update cmpOr()
        {
            this._boolMode = " OR ";
            return this;
        }

        public Update top(int Count)
        {
            this._top = Math.Abs(Count).ToString();
            return this;
        }

        public Update table(Raw FromRaw)
        {
            this._table = FromRaw.ToString();
            return this;
        }
        public Update table(string TableName)
        {
            this._table = String.Format("[{0}]", TableName);
            return this;
        }

        public Update set(Raw WhereRaw)
        {
            if (this._set != "") this._set += ", ";
            this._set += WhereRaw.ToString();
            return this;
        }
        public Update set(string Field, dynamic Value)
        {
            string raw = String.Format("[{0}] = {1}", Field, DB.val(Value));
            this.set(new Raw(raw));
            return this;
        }
        public Update set(System.Collections.Generic.Dictionary<string, dynamic> FieldsList)
        {
            foreach(var val in FieldsList)
            {
                this.set(val.Key, val.Value);
            }
            return this;
        }

        public Update from(Raw FromRaw)
        {
            this._from = FromRaw.ToString();
            return this;
        }
        public Update from(string TableName)
        {
            this._from = String.Format("[{0}]", TableName);
            return this;
        }
        public Update from(Select Select, string AsName = "")
        {
            this._from = String.Format("({0}) [{1}]", Select.ToString(), AsName);
            return this;
        }


        public Update where(Raw WhereRaw)
        {
            if (this._where != "") this._where += this._boolMode;
            this._where += WhereRaw.ToString();
            return this;
        }
        public Update where(string Field, string Comparator, dynamic Value)
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
