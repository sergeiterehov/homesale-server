
namespace homesale.App.Models
{
    class Client : Model<Client>
    {
        public long id;

        public string name;
        public string addres;
        public string phone;


        public Libs.ORM.Collection<Order> orders
        {
            get
            {
                return Order.List((s) => { return s.where("client_id", "=", id); });
            }
        }

        public Libs.ORM.Collection<Object> objects
        {
            get
            {
                return Object.List((s) => { return s.where("client_id", "=", id); });
            }
        }
    }
}
