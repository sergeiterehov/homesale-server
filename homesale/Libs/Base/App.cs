
namespace homesale.Libs.Base
{
    abstract class App
    {
        public HSP.Query Query;

        public dynamic Init(HSP.Query Query)
        {
            this.Query = Query;

            return this;
        }
    }
}
