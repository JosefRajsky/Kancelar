using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace CommandHandler
{
    public class Publisher : IPublisher
    {
        private ConnectionFactory _factory { get; set; }
        private IConnection _connection { get; set; }
        private IModel _channel { get; set; }
        private string _exchange { get; set; }
        private string _queue { get; set; }
        public async Task PushToExchange(string exchange,string message)
        {
            await Task.Run(() =>
            {
                    var body = Encoding.UTF8.GetBytes(message);
                     _channel.BasicPublish(
                     exchange: exchange,
                     routingKey: "",
                     basicProperties: null,
                     body: body);
            });
        }
        public async Task Push(string message) {

            await Task.Run(() =>
                {
                    var body = Encoding.UTF8.GetBytes(message);
                    _channel.TxSelect();

                    _channel.BasicPublish(
                      exchange: "eventstore.ex",
                      routingKey: "",
                      basicProperties: null,
                      body: body);  
                    _channel.BasicPublish(
                         exchange: _exchange,                        
                         routingKey: "",
                         basicProperties: null,
                         body: body);
                    _channel.TxCommit();
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
            IBasicProperties props = _channel.CreateBasicProperties();
            props.Expiration = "432000";
            var args = new Dictionary<string, object>();
            args.Add("x-message-ttl", 432000);
            _channel.ExchangeDeclare(_exchange, ExchangeType.Fanout,false,false,args);

        }


    }
}
