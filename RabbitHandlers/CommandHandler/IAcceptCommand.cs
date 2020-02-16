
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommandHandler
{
    public interface IAcceptCommand
    {
        EventingBasicConsumer Start();
        public void Stop();

       

    }
}
