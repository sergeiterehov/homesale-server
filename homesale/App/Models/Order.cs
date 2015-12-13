using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homesale.App.Models
{
    class Order : Model<Order>
    {
        public long id;

        public long client_id;
        public long agent_id;

        public DateTime data;

        public long payment_id;
        public DateTime data_complete;
    }
}
