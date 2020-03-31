
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommandHandler
{
    public class MessageHandler : IMessageHandler
    {
        public Publisher _publisher;
        public MessageHandler(Publisher publisher) {
            _publisher = publisher;
        }
        //public async Task<Message> MakeCommand<T>(T message, MessageType typ, Guid? parentGuid, int version, bool send)
        //{
        //    //var cmd = new Message() {
        //    //    Guid = Guid.NewGuid(),
        //    //    MessageType =typ,               
        //    //    Created = DateTime.Now,
        //    //    ParentGuid = parentGuid,
        //    //    Command = await Task.Run(() => JsonConvert.SerializeObject(message))
        //    //};
        //    //if (send) await _publisher.Push(await Task.Run(() => JsonConvert.SerializeObject(cmd)));
        //    //return cmd;
        //}

        public async Task PublishEvent<E>(E evt, MessageType typ,Guid guid, Guid? parentGuid, int generation, Guid entityId)
        {
            var ev = new Message()
            {
                Guid = guid,
                MessageType = typ,
                Created = DateTime.Now,
                ParentGuid = parentGuid,
                Event = await Task.Run(() => JsonConvert.SerializeObject(evt)),              
                EntityId = entityId,
                Generation = generation,
            };
            await _publisher.Push(await Task.Run(() => JsonConvert.SerializeObject(ev)));          
        }

        public Task PublishEvent<E>(E evt, MessageType typ, Guid? parentGuid, int generation, Guid entityId)
        {
            throw new NotImplementedException();
        }

        public async Task PublishEventToExchange<E>(E evt, MessageType typ, Guid guid, Guid? parentGuid, int generation, Guid entityId, string exchange)
        {
            var ev = new Message()
            {
                Guid = guid,
                MessageType = typ,
                Created = DateTime.Now,
                ParentGuid = parentGuid,
                Event = await Task.Run(() => JsonConvert.SerializeObject(evt)),
                EntityId = entityId,
                Generation = generation,
            };
            var msg = JsonConvert.SerializeObject(ev);
            await _publisher.PushToExchange(exchange, msg);
        }

      
    }
}
