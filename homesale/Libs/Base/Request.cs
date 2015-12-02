using System.Xml;

namespace homesale.Libs.Base
{
    abstract class Request<T> where T : Request<T>
    {
        public Response Response;

        public Request()
        {
            this.Response = new Response();

            this.Init(homesale.App.Main.ME().Query.XML);
        }

        public T THIS
        {
            get
            {
                return (T)(dynamic)this;
            }
        }

        private T Init(XmlReader XML)
        {
            XML.ReadToFollowing("data");

            foreach (System.Reflection.FieldInfo Field in this.GetType().GetFields())
            {
                if(Field.DeclaringType == this.GetType() && XML.ReadToFollowing(Field.Name.ToLower()))
                {
                    object value = XML.ReadElementContentAs(Field.FieldType, null);
                    Field.SetValue(this, value);
                }
            }

            return THIS;
        }
    }
}
