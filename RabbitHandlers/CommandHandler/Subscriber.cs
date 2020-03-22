﻿using System;
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
        public EventingBasicConsumer Start()
        {
            foreach (var ex in _exchange)
            {
                var args = new Dictionary<string, object>();
                args.Add("x-message-ttl", 432000);
                _channel.ExchangeDeclare(exchange: ex, type: ExchangeType.Fanout,false,false,args);
            }
            //_channel.BasicPublish(
            // exchange: _exchange,
            // routingKey: "",
            // basicProperties: null,
            // body: null);

            //var queueName = _channel.QueueDeclare().QueueName;
            //foreach (var ex in _exchange)
            //{
            //    _channel.QueueBind(queue: queueName,
            //                  exchange: ex,
            //                  routingKey: "");
            //}

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
            };
            _channel.BasicConsume(queue: _queueName,
                                 autoAck: true,
                                 consumer: consumer);
            return consumer;
        }
        public void Stop()
        {
            this._connection.Close();
        }
        public Subscriber(List<string> exchange, IConnection conn, IModel channel, string queue)
        {
            this._exchange = exchange;
            this._connection = conn;
            this._channel = channel;
            this._queueName = queue;
        }
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
