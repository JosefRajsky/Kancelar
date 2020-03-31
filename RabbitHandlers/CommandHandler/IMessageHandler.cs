
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommandHandler
{
    public interface IMessageHandler
    {
        //Task<Message> MakeCommand<T>(T message, MessageType typ, Guid? parentGuid, int version, bool send);
        Task PublishEvent<E>(E evt, MessageType typ, Guid? parentGuid, int generation, Guid entityId);
        Task PublishEventToExchange<E>(E evt, MessageType typ, Guid guid, Guid? parentGuid, int generation, Guid entityId,string exchange);

    }
}
