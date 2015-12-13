using System;

namespace homesale.App.Models
{
    class Object : Model<Object>
    {
        public long id;

        public long client_id;
        public long type_id;

        public DateTime date;


        public Client client
        {
            get
            {
                return Client.Find(client_id);
            }
            set
            {
                client_id = value.id;
            }
        }

        public ObjectType type
        {
            get
            {
                return ObjectType.Find(type_id);
            }
            set
            {
                type_id = value.id;
            }
        }

        public Libs.ORM.Collection<ObjectCall> calls
        {
            get
            {
                return HasMany<ObjectCall>();
            }
        }
    }
}
