
namespace homesale.App.Models
{
    class ObjectType : Model<ObjectType>
    {
        public long id;

        public string name;
        public string description;


        public Libs.ORM.Collection<Object> objects
        {
            get
            {
                return Object.List((s)=> { return s.where("type_id", "=", id); });
            }
        }
    }
}
