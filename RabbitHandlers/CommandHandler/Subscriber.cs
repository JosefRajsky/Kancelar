using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandHandler
{
    public class Subscriber : ISubscriber
    {
        ConnectionFactory factory { get; set; }
        IConnection connection { get; set; }
        IModel _channel { get; set; }
        string _exchange { get; set; }
        public EventingBasicConsumer Start()
        {
            
            _channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Fanout);
     
                _channel.BasicPublish(
                 exchange: _exchange,
                 routingKey: "",
                 basicProperties: null,
                 body: null);
           
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName,
                              exchange: _exchange,
                              routingKey: "");
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
            };
            _channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
            return consumer;
        }
        public void Stop()
        {
            this.connection.Close();
        }

        public Subscriber(ConnectionFactory connectionFactory, string exchange)
        {
            this._exchange = exchange;
            this.factory = connectionFactory;
            this.connection = factory.CreateConnection();
            this._channel = connection.CreateModel();


        }

    }
}
