using System;
using System.Collections.Generic;

namespace homesale.Libs.ORM
{
    class Collection<ModelType> where ModelType : Model<ModelType>, new()
    {
        private Select selector;
        private List<ModelType> list = new List<ModelType>();

        public Collection()
        {

        }
        public Collection(Select SelectQuery)
        {
            this.selector = SelectQuery;
        }
        public Collection(System.Data.DataTable Table)
        {
            this.Add(Table);
        }

        static public string GetTableName()
        {
            return Model<ModelType>.GetTableName();
        }

        public Collection<ModelType> Get()
        {
            if(this.selector != null) this.Add(this.selector);
            return this;
        }

        public Collection<ModelType> Select(Func<Select, Select> SelectFunction)
        {
            this.selector = SelectFunction(new Select(GetTableName()));
            return this;
        }

        public Collection<ModelType> Add(Select Selector)
        {
            var result = Selector.Get();
            foreach (System.Data.DataRow row in result.Rows)
            {
                this.Add(row);
            }
            return this;
        }

        public Collection<ModelType> Add(System.Data.DataTable Table)
        {
            foreach (System.Data.DataRow row in Table.Rows)
            {
                this.Add(row);
            }
            return this;
        }

        public Collection<ModelType> Add(System.Data.DataRow Row)
        {
            this.Add(new ModelType().FromRow(Row));
            return this;
        }

        public Collection<ModelType> Add(ModelType Element)
        {
            this.list.Add(Element);
            return this;
        }

        public List<ModelType> GetList()
        {
            return this.list;
        }

        public ModelType this[int Key]
        {
            get
            {
                return this.list[Key];
            }
            set
            {
                this.list[Key] = value;
            }
        }

        public Collection<ModelType> Delete()
        {
            foreach(var obj in this.list)
            {
                obj.Delete();
                this.list.Remove(obj);
            }
            return this;
        }

        public Collection<ModelType> Save()
        {
            foreach (var obj in this.list)
            {
                obj.Save();
            }
            return this;
        }

        public ModelType First()
        {
            return this.list.Count > 0 ? this.list[0] : null;
        }

        public Collection<ModelType> val(string FieldName, object Value)
        {
            var field = typeof(ModelType).GetField(FieldName);
            foreach (var obj in this.list)
            {
                field.SetValue(obj, Value);
            }
            return this;
        }

        public Collection<ModelType> Each(Func<ModelType, ModelType> EachFunction)
        {
            foreach (var obj in this.list)
            {
                EachFunction(obj);
            }
            return this;
        }
        public Collection<ModelType> Each(string FieldName, Func<object, object> EachFunction)
        {
            var field = typeof(ModelType).GetField(FieldName);
            foreach (var obj in this.list)
            {
                field.SetValue(obj, EachFunction(field.GetValue(obj)));
            }
            return this;
        }

        public int Count()
        {
            return this.list.Count;
        }

        public double Avg(string FieldName)
        {
            double result = 0;
            this.Each(FieldName, (val) => { result += Convert.ToDouble(val);  return val; });
            return result / this.Count();
        }

        public double Sum(string FieldName)
        {
            double result = 0;
            this.Each(FieldName, (val) => { result += Convert.ToDouble(val); return val; });
            return result;
        }

        public double Min(string FieldName)
        {
            if (this.list.Count == 0) return 0;
            double result = Convert.ToDouble(this.First().val(FieldName));
            this.Each(FieldName, (val) => { result = Convert.ToDouble(val) < result ? Convert.ToDouble(val) : result; return val; });
            return result;
        }

        public double Max(string FieldName)
        {
            if (this.list.Count == 0) return 0;
            double result = Convert.ToDouble(this.First().val(FieldName));
            this.Each(FieldName, (val) => { result = Convert.ToDouble(val) > result ? Convert.ToDouble(val) : result; return val; });
            return result;
        }
    }
}
