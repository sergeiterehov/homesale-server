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
            this._table = this.GetType().Name.ToLower() + "s";
            this._selector = new Select(this._table).top(1).select(new Raw("*"));
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

        public T Save()
        {
            Dictionary<string, dynamic> fields = new Dictionary<string, dynamic>();

            foreach (var prop in this.GetType().GetFields())
                if (prop.IsPublic && !prop.IsStatic && prop.DeclaringType == this.GetType() && prop.Name != "id")
                {
                    fields.Add(prop.Name, prop.GetValue(this));
                }

            if (this.GetType().GetProperty("id") == null)
            {
                // insert
            }
            else
            {
                // update
                new Update();
            }

            return THIS;
        }

        //-----------------------> STATIC

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
    }
}
