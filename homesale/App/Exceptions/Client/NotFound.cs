using System;

namespace homesale.App.Exceptions.Client
{
    class NotFound : Exception
    {
        public NotFound() : base("Пользователь не найден!")
        {

        }
    }
}
