
namespace homesale.App.Controllers
{
    class Client : Controller
    {
        public XML Get(Requests.Client.Get request)
        {
            var client = Models.Client.Find(request.id);

            if (client != null)
            {
                return new XML().xml(client.Assoc.InnerString);
            }
            else
            {
                throw new Exceptions.Client.NotFound();
            }
        }

        public XML GetList()
        {
            var clients = Models.Client.All().Get();

            var result = new XML("list");

            foreach(var client in clients.GetList())
            {
                result.append(
                    XML.Get("item", client.Assoc.InnerString)
                );
            }

            return new XML().append(result);
        }

        public XML Create(Requests.Client.Create request)
        {
            var client = new Models.Client();

            client.From(request);

            client.Save();

            if (client.id > 0)
            {
                return new XML().add("id", client.id);
            }
            else
            {
                throw new Exceptions.Client.CanNotCreate();
            }
        }

        public XML Edit(Requests.Client.Edit request)
        {
            var client = Models.Client.Find(request.id);

            if(client != null)
            {
                client.From(request);

                client.Save();

                return new XML();
            }
            else
            {
                throw new Exceptions.Client.NotFound();
            }
        }
        
        // todo
        public XML GetServices(Requests.Client.GetServices request)
        {
            var client = Models.Client.Find(request.id);

            if (client != null)
            {
                //----> Получить историю услуг

                return new XML();
            }
            else
            {
                throw new Exceptions.Client.NotFound();
            }
        }

        public XML GetOrders(Requests.Client.GetOrders request)
        {
            var client = Models.Client.Find(request.id);

            if(null != client)
            {
                var orders = client.orders.Get();
                var result = new XML("list");

                foreach(var item in orders.GetList())
                {
                    result.add("item", item.Assoc.InnerString);
                }

                return new XML().append(result);
            }
            else
            {
                throw new Exceptions.Client.NotFound();
            }
        }

        public XML GetObjects(Requests.Client.GetObjects request)
        {
            var client = Models.Client.Find(request.id);

            if (null != client)
            {
                var objects = client.objects.Get();
                var result = new XML("list");

                foreach (var item in objects.GetList())
                {
                    result.add("item", item.Assoc.InnerString);
                }

                return new XML().append(result);
            }
            else
            {
                throw new Exceptions.Client.NotFound();
            }
        }
    }
}
