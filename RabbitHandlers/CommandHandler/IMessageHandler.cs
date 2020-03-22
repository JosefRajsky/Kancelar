using EventLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommandHandler
{
    public interface IMessageHandler
    {
        Task<Message> MakeCommand<T>(T message, MessageType typ, Guid? parentGuid, int version, bool send);
        Task<Guid> PublishEvent<E,C>(E message,C cmd, MessageType typ, Guid? parentGuid, int generation, Guid entityId);

    }
}
