using System.Xml.Linq;

namespace homesale.Libs.Base
{
    abstract class Request<T> : Assoc where T : Request<T>
    {
        public Response Response;

        public Request()
        {
            this.Response = new Response();

            this.Init(App.Main.ME().Query.XML);

            typeof(T).GetMethod("Rule").Invoke(THIS, new object[] { });
        }

        public T THIS
        {
            get
            {
                return (T)(dynamic)this;
            }
        }

        public void Rule()
        {

        }

        private T Init(XDocument XML)
        {
            var data = XML.Element(XName.Get("call"));

            this.FromXml(data.Elements());

            foreach (var Field in this.GetType().GetFields())
            {
                var xfield = data.Element(XName.Get(Field.Name));
                if (Field.DeclaringType == this.GetType())
                    if(xfield != null)
                    {
                        if(Field.FieldType.BaseType == typeof(Assoc))
                        {
                            Field.SetValue(this, new Assoc().FromXml(xfield.Elements()));
                        }
                        else
                        {
                            object value = System.Convert.ChangeType(xfield.Value, Field.FieldType);
                            Field.SetValue(this, value);
                        }
                    }
                    else
                    {
                        if(0 == ((RequestAttributes.Option[])Field.GetCustomAttributes(typeof(RequestAttributes.Option), false)).Length)
                        {
                            App.Main.Abort("Ожидался параметр \"" + Field.Name + "\" типа \"" + Field.FieldType.Name + "\"");
                        }
                    }
            }

            return THIS;
        }
    }
}
