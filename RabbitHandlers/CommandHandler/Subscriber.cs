using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace CommandHandler
{
    public class Subscriber : ISubscriber
    {
        ConnectionFactory _factory { get; set; }
        IConnection _connection { get; set; }
        IModel _channel { get; set; }
        string _exchange { get; set; }
        public EventingBasicConsumer Start()
        {
            
            _channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Fanout);
     
                //_channel.BasicPublish(
                // exchange: _exchange,
                // routingKey: "",
                // basicProperties: null,
                // body: null);
           
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
            this._connection.Close();
        }

        public Subscriber(ConnectionFactory connectionFactory, string exchange)
        {
            this._exchange = exchange;
            this._factory = connectionFactory;
            this._factory.AutomaticRecoveryEnabled = true;
            this._factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);
            try
            {
                this._connection = _factory.CreateConnection();
            }
            catch (BrokerUnreachableException e)
            {
                Thread.Sleep(5000);
                this._connection = _factory.CreateConnection();
            }
            this._connection = _factory.CreateConnection();
            this._channel = _connection.CreateModel();


        }

    }
}
