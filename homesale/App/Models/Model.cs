using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homesale.App.Models
{
    abstract class Model<T> : Libs.ORM.Model<T> where T : Libs.ORM.Model<T>, new()
    {
    }
}
