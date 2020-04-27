using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.Framing.Impl;

namespace CommandHandler
{
    public class Subscriber : ISubscriber
    {
        ConnectionFactory _factory { get; set; }
        IConnection _connection { get; set; }
        IModel _channel { get; set; }
        List<string> _exchange { get; set; }
        string _queueName { get; set; }
        //Description: Metoda zahájení a přihlášení posluchače ke konzumaci
        public EventingBasicConsumer Start()
        {
            //Description: Ověření platnosti všech exchange, případně jejich vytvoření
            foreach (var ex in _exchange)
            {
                var args = new Dictionary<string, object>();
                args.Add("x-message-ttl", 432000);
                _channel.ExchangeDeclare(exchange: ex, type: ExchangeType.Fanout,false,false,args);
            }
            //Description: Nastavení kanálu pro konzumaci zpráv
            var consumer = new EventingBasicConsumer(_channel);
            //Description: Reakce na příchozí zprávu
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
            };
            //Description: Nastavení konzumačního kanálu na frontu frontu servisu.
            _channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);
            return consumer;
        }
        //Description: Metoda pro zastavení konzumace
        public void Stop()
        {
            this._connection.Close();
        }
        //Description: vytvoření objektu pro odebírání zpráv z množiny Exchange
        public Subscriber(List<string> exchange, IConnection conn, IModel channel, string queue)
        {
            this._exchange = exchange;
            this._connection = conn;
            this._channel = channel;
            this._queueName = queue;
        }
        //Description: vytvoření objektu pro odebírání zpráv z konkrétního Exchange
        public Subscriber(string exchange, IConnection conn, IModel channel, string queue)
        {
            var exchanges = new List<string>();
            exchanges.Add(exchange);
            this._exchange = exchanges;
            this._connection = conn;
            this._channel = channel;
            this._queueName = queue;
        }


    }
}
