using Dochazka_Service.Repositories;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using Udalost_Service;

namespace Dochazka_Service.Handlers
{
    public class EventBroker : IEventBroker
    {
        ConnectionFactory factory { get; set; }
        private readonly IServiceScopeFactory scopeFactory;
        IConnection connection { get; set; }
        IModel channel { get; set; }


        public void Start()
        {
            channel.ExchangeDeclare(exchange: "dochazka.ex", type: ExchangeType.Fanout);
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName,
                              exchange: "dochazka.ex",
                              routingKey: "");
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                //Po přijmutí Eventu vytvořit záznam;
                var repository = new DochazkaRepository();
                repository.AddMessage(message);
               
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }
        public void Stop()
        {
            this.connection.Close();
        }

        public EventBroker(IServiceScopeFactory scopeFactory)  
        {
            this.factory = new ConnectionFactory() { HostName = "rabbitmq" };
            this.connection = factory.CreateConnection();
            this.channel = connection.CreateModel();
            

        }
    }
}
