using System.Xml.Linq;

namespace homesale.Libs.Base
{
    abstract class Request<T> where T : Request<T>
    {
        public Response Response;

        public Request()
        {
            this.Response = new Response();

            this.Init(App.Main.ME().Query.XML);
        }

        public T THIS
        {
            get
            {
                return (T)(dynamic)this;
            }
        }

        private T Init(XDocument XML)
        {
            var data = XML.Element(XName.Get("call"));

            foreach (var Field in this.GetType().GetFields())
            {
                var xfield = XML.Element(XName.Get(Field.Name.ToLower()));
                if (Field.DeclaringType == this.GetType() && xfield != null)
                {
                    object value = System.Convert.ChangeType(xfield.Value, Field.FieldType); ;
                    Field.SetValue(this, value);
                }
            }

            return THIS;
        }
    }
}
