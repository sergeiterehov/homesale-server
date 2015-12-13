using System;

namespace homesale.App.Exceptions.ObjectType
{
    class NotFound : Exception
    {
        public NotFound() : base("Тип объекта не найден!") { }
    }
}
