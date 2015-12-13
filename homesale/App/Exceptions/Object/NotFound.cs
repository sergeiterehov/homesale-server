using System;

namespace homesale.App.Exceptions.Object
{
    class NotFound : Exception
    {
        public NotFound() : base("Объект не найден!") { }
    }
}
