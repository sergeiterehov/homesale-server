using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homesale.App.Requests
{
    class Request<T> : Libs.Base.Request<T> where T : Libs.Base.Request<T>, new()
    {
    }
}
