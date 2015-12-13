using System;

namespace homesale.App.Exceptions.Client
{
    class CanNotCreate : Exception
    {
        public CanNotCreate() : base("Невозможно создать клиента!") { }
    }
}
