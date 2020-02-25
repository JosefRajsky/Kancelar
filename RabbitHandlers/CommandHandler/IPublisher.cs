
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CommandHandler
{
    public interface IPublisher
    {
        Task<bool> Push(string message);

        Task<bool> PushToStore(string message);

        EventingBasicConsumer Start();
    }
}
