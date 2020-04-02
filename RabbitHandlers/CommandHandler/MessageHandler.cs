
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
        public async Task RequestReplay(string exhange,Guid? entityId, List<MessageType> msgTypes) {
            var evt = new ProvideHealingStream() { Exchange = "uzivatel.ex", MessageTypes = msgTypes, EntityId = entityId };
            var msg = new Message()
            {
                Guid = Guid.NewGuid(),
                MessageType = MessageType.ProvideHealingStream,
                Created = DateTime.Now,
                EntityId = (entityId == Guid.Empty) ? Guid.Empty : Guid.Parse(entityId.ToString()),
                ParentGuid = null,
                Event = await Task.Run(() => JsonConvert.SerializeObject(evt))
            };
            await _publisher.Push(JsonConvert.SerializeObject(msg));
        }
      
    }
}
