using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homesale.Libs.ORM
{
    class Raw
    {
        private string _raw;

        public Raw(string RawSqlString)
        {
            this._raw = RawSqlString;
        }

        public override string ToString()
        {
            return this._raw;
        }
    }
}
