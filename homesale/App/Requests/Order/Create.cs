using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homesale.App.Requests.Order
{
    class Create : Request<Create>
    {
        public long clientId;

        public double price;
        [Option] public string description;

        new public void Rule()
        {
            if (price <= 0) throw Main.Abort("Стоимость должна быть положительной!");
        }
    }
}
