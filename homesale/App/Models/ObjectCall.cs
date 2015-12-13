
namespace homesale.App.Models
{
    class ObjectCall : Model<ObjectCall>
    {
        public long id;

        public long object_id;
        public long position_id;

        public string value;


        public Object owner
        {
            get
            {
                return Object.Find(object_id);
            }
            set
            {
                object_id = owner.id;
            }
        }

        public Position position
        {
            get
            {
                return Position.Find(position_id);
            }
            set
            {
                position_id = position.id;
            }
        }

    }
}
