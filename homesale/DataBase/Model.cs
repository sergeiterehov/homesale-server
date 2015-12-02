using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homesale.DataBase
{
    abstract class Model
    {
        protected DB DB;

        private string CLASS_NAME;

        protected string TABLE_NAME;
        protected string PRIMARY = "id";

        public Model()
        {
            this.DB = DB.ME();

            // Сохраняем название класса (модели)
            this.CLASS_NAME = this.GetType().Name;
            // Определеяем назване базы данных (User -> users)
            this.TABLE_NAME = this.CLASS_NAME.ToLower() + "s";
        }

        //------------------> Active

        public Model Save()
        {
            List<string> fields = new List<string>();
            List<object> values = new List<object>();


            foreach (var prop in this.GetType().GetFields())
                if(prop.IsPublic && !prop.IsStatic && prop.Name != this.PRIMARY)
                {
                    fields.Add(prop.Name);
                    values.Add(prop.GetValue(this));
                }

            if (this.GetType().GetProperty(this.PRIMARY) == null)
            {
                this.DB.Insert(this.TABLE_NAME, fields.ToArray(), values.ToArray());
            }
            else
            {
                this.DB.Update(this.TABLE_NAME, this.PRIMARY, "=", this.GetType().GetProperty(this.PRIMARY), fields.ToArray(), values.ToArray());
            }


            return this;
        }
    }
}
