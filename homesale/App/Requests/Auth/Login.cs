namespace homesale.App.Requests.Auth
{
    class Login : Request<Login>
    {
        public string login;
        public string passwd;
    }
}
