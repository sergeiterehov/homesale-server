
namespace homesale.App.Requests.Client
{
    class Edit : Request<Edit>
    {
        public long id;

        [Option] public string name;
        [Option] public string addres;
        [Option] public string phone;
    }
}
