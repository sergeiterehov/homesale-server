
namespace homesale.App.Models
{
    class Agent : Model<Agent>
    {
        public long id;

        public string login;
        public string passwd;

        public string name;

        public string addres;
        public string phone;

        public decimal money;
    }
}
