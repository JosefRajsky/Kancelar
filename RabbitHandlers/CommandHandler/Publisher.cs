using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventLibrary;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace CommandHandler
{
    public class Publisher
    {
        private ConnectionFactory _factory { get; set; }
        private IConnection _connection { get; set; }
        private IModel _channel { get; set; }
        private string _exchange { get; set; }
        private string _queue { get; set; }
        public async Task Push(string message) {
           

                await Task.Run(() =>
                {
                    var body = Encoding.UTF8.GetBytes(message);
                    
                  _channel.BasicPublish(
                         exchange: _exchange,
                         routingKey: "",
                         basicProperties: null,
                         body: body);
                    //var queueName = _channel.QueueDeclare(_queue).QueueName;
                    //_channel.QueueBind(queue: queueName,
                    //              exchange: _exchange,
                    //              routingKey: "");
                   
                });
          
           
        }
        public async Task PushToStore(string message)
        {


            await Task.Run(() =>
            {
                var body = Encoding.UTF8.GetBytes(message);

                _channel.BasicPublish(
                       exchange: "eventstore.ex",
                       routingKey: "",
                       basicProperties: null,
                       body: body);               

            });


        }
        public Publisher(ConnectionFactory connectionFactory, string exchange,string queue)
        {
            this._exchange = exchange;
            this._queue = queue;
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
            this._channel = _connection.CreateModel();
            _channel.ExchangeDeclare(_exchange, ExchangeType.Fanout);

        }

    }
}
