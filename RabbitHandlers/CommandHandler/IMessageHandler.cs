using EventLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommandHandler
{
    public interface IMessageHandler
    {
        Task<Guid> MakeCommand<T>(T message, MessageType typ, Guid? parentGuid, int version, bool send);
        Task<Guid> PublishEvent<T>(T message, MessageType typ, Guid? parentGuid, int version, bool send);

    }
}
