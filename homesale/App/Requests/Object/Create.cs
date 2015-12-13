using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homesale.App.Requests.Object
{
    class Create : Request<Create>
    {
        public long clientId;

        public long typeId;

        [Option] public Assoc calls;


        new public void Rule()
        {
            if (Has("calls"))
            {
                foreach(var item in calls.GetList())
                {
                    if(!(
                        item.Has("positionId") &&
                        item.Has("value")
                    ))
                    {
                        throw Main.Abort("Ошибка в структуре характеристик!");
                    }
                }
            }
        }
    }
}
