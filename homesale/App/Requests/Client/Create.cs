using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homesale.App.Requests.Client
{
    class Create : Request<Create>
    {
        [Option] public string name;
        [Option] public string addres;
        [Option] public string phone;
    }
}
