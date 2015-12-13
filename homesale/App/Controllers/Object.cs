using System;

namespace homesale.App.Controllers
{
    class Object : Controller
    {
        public XML Create(Requests.Object.Create request)
        {
            var client = Models.Client.Find(request.clientId);

            if(client != null)
            {
                var type = Models.ObjectType.Find(request.typeId);

                if(type != null)
                {
                    var agent = Auth.Agent;
                    var newObject = new Models.Object();

                    newObject.client = client;
                    newObject.type = type;
                    newObject.date = new DateTime();

                    newObject.Save();

                    if(newObject.id > 0)
                    {
                        if (request.Has("calls"))
                        {
                            foreach(var call in request.calls.GetList())
                            {
                                var position = Models.Position.Find(call.Get<long>("positionId"));

                                if(position != null)
                                {
                                    var newCall = new Models.ObjectCall();

                                    newCall.owner = newObject;
                                    newCall.position = position;
                                    newCall.value = call.Get<string>("value");

                                    newCall.Save();
                                }
                            }
                        }

                        return new XML().add("id", newObject.id);
                    }
                    else
                    {
                        throw new Exceptions.Object.CanNotCreate();
                    }
                }
                else
                {
                    throw new Exceptions.ObjectType.NotFound();
                }
            }
            else
            {
                throw new Exceptions.Client.NotFound();
            }
        }
    }
}
