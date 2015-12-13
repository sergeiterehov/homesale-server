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

        public long ID
        {
            get
            {
                return (long) this.GetType().GetField("id").GetValue(this);
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

            long id = this.ID;

            foreach (var prop in this.GetType().GetFields())
                if (prop.IsPublic && !prop.IsStatic && prop.DeclaringType == this.GetType() && prop.Name != "id")
                {
                    var val = prop.GetValue(this);
                    if (null != val)
                    {
                        values.Add(val.ToString());
                        fields.Add(prop.Name, prop.GetValue(this));
                        columns.Add(prop.Name);
                    }
                }

            if (id == 0)
            {
                this.ID = new Insert(this._table).column(columns.ToArray()).value(values.ToArray()).Do();
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
            this.ID = 0;

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

        public bool HasAttribute(string FieldName, Type AttributeType)
        {
            return HasAttribute(this.GetType().GetField(FieldName), AttributeType);
        }
        public bool HasAttribute(System.Reflection.FieldInfo Field, Type AttributeType)
        {
            return (0 < ((dynamic)Field.GetCustomAttributes(AttributeType, false)).Length);
        }

        public Assoc ToAssoc()
        {
            var result = new Assoc(this.GetType().Name);

            foreach (var field in this.GetType().GetFields())
            {
                if (
                    false == HasAttribute(field, typeof(ModelAttributes.Inside))
                )
                {
                    result.Add(field.Name, field.GetValue(this));
                }
            }

            return result;
        }
        public Assoc Assoc
        {
            get
            {
                return this.ToAssoc();
            }
            set
            {
                this.From(value);
            }
        }

        public T From(Assoc Fields)
        {
            foreach(var field in this.GetType().GetFields())
            {
                if (Fields.Has(field.Name))
                {
                    if (
                        false == HasAttribute(field, typeof(ModelAttributes.Personal)) &&
                        false == HasAttribute(field, typeof(ModelAttributes.Inside)) &&
                        field.Name != "id"
                    )
                    {
                        field.SetValue(this, Fields[field.Name, field.FieldType]);
                    }
                }
            }

            return THIS;
        }


        public Collection<ModelType> HasMany<ModelType>() where ModelType : Model<ModelType>, new()
        {
            return HasMany<ModelType>(GetTableName() + "_id");
        }
        public Collection<ModelType> HasMany<ModelType>(string PK, string FK = "id") where ModelType : Model<ModelType>, new()
        {
            Func<Select, Select> func = (s) => { return s.where(FK, "=", typeof(T).GetField(PK).GetValue(this)); };
            return typeof(ModelType).GetMethod("List").Invoke(null, new object[] { func }) as Collection<ModelType>;
        }

        public ModelType HasOne<ModelType>() where ModelType : Model<ModelType>, new()
        {
            return HasOne<ModelType>(GetTableName() + "_id");
        }
        public ModelType HasOne<ModelType>(string PK, string FK = "id") where ModelType : Model<ModelType>, new()
        {
            Func<Select, Select> func = (s) => { return s.where(FK, "=", typeof(T).GetField(PK).GetValue(this)); };
            return typeof(ModelType).GetMethod("By").Invoke(null, new object[] { func }) as ModelType;
        }

        public Collection<ModelType> HasManyOwner<ModelType>() where ModelType : Model<ModelType>, new()
        {
            return HasManyOwner<ModelType>(typeof(ModelType).GetMethod("GetTableName").Invoke(null, new object[] { }) + "_id");
        }
        public Collection<ModelType> HasManyOwner<ModelType>(string FK, string PK = "id") where ModelType : Model<ModelType>, new()
        {
            Func<Select, Select> func = (s) => { return s.where(PK, "=", typeof(T).GetField(FK).GetValue(this)); };
            return typeof(ModelType).GetMethod("List").Invoke(null, new object[] { func }) as Collection<ModelType>;
        }

        public ModelType HasOneOwner<ModelType>() where ModelType : Model<ModelType>, new()
        {
            return HasOneOwner<ModelType>(typeof(ModelType).GetMethod("GetTableName").Invoke(null, new object[] { }) + "_id");
        }
        public ModelType HasOneOwner<ModelType>(string FK, string PK = "id") where ModelType : Model<ModelType>, new()
        {
            Func<Select, Select> func = (s) => { return s.where(PK, "=", typeof(T).GetField(FK).GetValue(this)); };
            return typeof(ModelType).GetMethod("By").Invoke(null, new object[] { func }) as ModelType;
        }


        //-----------------------> STATIC

        static public string GetTableName()
        {
            return System.Text.RegularExpressions.Regex.Replace(
                typeof(T).Name + "s", @".+[A-Z]\w.",
                delegate (System.Text.RegularExpressions.Match match)
                {
                    return "_" + match.ToString();
                }
            );
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

        static public Collection<T> All()
        {
            return new Collection<T>(new Select(GetTableName()));
        }

        static public T Find(long Id)
        {
            return By((select)=> { return select.where("id", "=", Id); }).Get();
        }
    }
}
