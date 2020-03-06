using EventLibrary;
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
        public async Task<Guid> MakeCommand<T>(T message, MessageType typ, Guid? parentGuid, int version, bool send)
        {
            var cmd = new Message() {
                Guid = Guid.NewGuid(),
                MessageType =typ,
                Version = version,
                Created = DateTime.Now,
                ParentGuid = parentGuid,
                Body = await Task.Run(() => JsonConvert.SerializeObject(message))
            };
            if (send) await _publisher.Push(await Task.Run(() => JsonConvert.SerializeObject(cmd)));
            return cmd.Guid;
        }

        public async Task<Guid> PublishEvent<T>(T message, MessageType typ, Guid? parentGuid, int version, bool send)
        {
            var ev = new Message()
            {
                Guid = Guid.NewGuid(),
                MessageType = typ,
                Version = version,
                Created = DateTime.Now,
                ParentGuid = parentGuid,
                Body = await Task.Run(() => JsonConvert.SerializeObject(message))
        };

            if (send) await _publisher.Push(await Task.Run(() => JsonConvert.SerializeObject(ev)));
            return ev.Guid;
        }

  
    }
}
