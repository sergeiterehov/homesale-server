using System;

namespace homesale.App.Exceptions.Object
{
    class CanNotCreate : Exception
    {
        public CanNotCreate() : base("Невозможно создать объект!") { }
    }
}
