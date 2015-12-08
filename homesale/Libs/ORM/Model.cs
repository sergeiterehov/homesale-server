using System;
using System.Collections.Generic;
using System.Data;

namespace homesale.Libs.ORM
{
    abstract class Model<T> where T : Model<T>, new()
    {
        protected string _table;
        private Select _selector;
        private DataRow _data;

        private T THIS
        {
            get
            {
                return (T)(dynamic)this;
            }
        }

        public Model()
        {
            this._table = GetTableName();
            this._selector = new Select(this._table).top(1).select(new Raw("*"));
        }

        public T FromRow(DataRow Row)
        {
            foreach (var prop in this.GetType().GetFields())
                if (prop.IsPublic && !prop.IsStatic && prop.DeclaringType == this.GetType())
                {
                    prop.SetValue(this, Row[prop.Name]);
                }
            return THIS;
        }

        public Select Selector()
        {
            return this._selector;
        }

        public T Select(Func<Select, Select> SelectFunction)
        {
            SelectFunction(this.Selector());
            return THIS;
        }

        public T Get()
        {
            DataTable result = this._selector.Get();

            if (result.Rows.Count > 0)
            {
                DataRow row = result.Rows[0];

                foreach (var prop in this.GetType().GetFields())
                    if (prop.IsPublic && !prop.IsStatic && prop.DeclaringType == this.GetType())
                    {
                        prop.SetValue(this, row[prop.Name]);
                    }

                return THIS;
            }
            else
            {
                return null;
            }
        }

        public object ID
        {
            get
            {
                return this.GetType().GetField("id").GetValue(this);
            }
            set
            {
                this.GetType().GetField("id").SetValue(this, value);
            }
        }

        public T Save()
        {
            Dictionary<string, dynamic> fields = new Dictionary<string, dynamic>();
            List<string> columns = new List<string>();
            List<string> values = new List<string>();

            object id = this.ID;

            foreach (var prop in this.GetType().GetFields())
                if (prop.IsPublic && !prop.IsStatic && prop.DeclaringType == this.GetType() && prop.Name != "id")
                {
                    fields.Add(prop.Name, prop.GetValue(this));
                    columns.Add(prop.Name);
                }

            if (id == null)
            {
                this.ID = new Insert(this._table).column(columns.ToArray()).value(values.ToArray()).top(1).Do();
            }
            else
            {
                new Update(this._table).set(fields).where("id", "=", id).Do();
            }

            return THIS;
        }

        public T Delete()
        {
            new Delete(this._table).where("id", "=", this.ID);
            this.ID = null;

            return THIS;
        }

        public object val(string FieldName)
        {
            return typeof(T).GetField(FieldName).GetValue(this);
        }
        public T val(string FieldName, object Value)
        {
            typeof(T).GetField(FieldName).SetValue(this, Value);
            return THIS;
        }

        //-----------------------> STATIC

        static public string GetTableName()
        {
            return typeof(T).Name.ToLower() + "s";
        }

        static public T By(Func<Select, Select> SelectFunction)
        {
            T obj = new T();
            obj.Select(SelectFunction);
            return obj;
        }

        static public T New()
        {
            return new T();
        }

        static public Collection<T> List(Func<Select, Select> SelectFunction)
        {
            return new Collection<T>(SelectFunction(new Select(GetTableName())));
        }
        static public Collection<T> List()
        {
            return new Collection<T>();
        }

        static public T Find(long Id)
        {
            return By((select)=> { return select.where("id", "=", Id); }).Get();
        }
    }
}
