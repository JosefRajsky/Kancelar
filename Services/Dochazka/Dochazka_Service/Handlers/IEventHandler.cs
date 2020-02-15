using Dochazka_Service.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Udalost_Service;

namespace Dochazka_Service.Handlers
{
    public interface IEventBroker
    {
        void Start();
        public void Stop();
        
    }
}
