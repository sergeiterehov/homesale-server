using System;

namespace homesale.App.Controllers
{
    class Order : Controller
    {
        public XML Create(Requests.Order.Create request)
        {
            var client = Models.Client.Find(request.clientId);
            var agent = Auth.Agent;

            if(null != client)
            {
                var payment = new Models.Payment();

                payment.price = request.price;
                payment.date = new DateTime();
                if (request.Has("description")) payment.description = request.description;

                payment.Save();

                var order = new Models.Order();

                order.agent_id = agent.id;
                order.client_id = client.id;

                order.data = payment.date;
                order.payment_id = payment.id;

                order.Save();

                return new XML()
                    .add("id", order.id)
                ;
            }
            else
            {
                throw Main.Abort("Клиент не найден!");
            }
        }
    }
}
