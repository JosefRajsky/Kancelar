﻿using System;
using System.Collections.Generic;
using System.Text;
using EventLibrary;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommandHandler
{
    public class PublishCommand
    {
        private ConnectionFactory _factory { get; set; }
        private IConnection _connection { get; set; }
        private IModel _channel { get; set; }
        private string _exchange { get; set; }
        public bool Push(string message) {
  
                var body = Encoding.UTF8.GetBytes(message);                
                _channel.ExchangeDeclare(_exchange, ExchangeType.Fanout);

            _channel.BasicPublish(
                 exchange: _exchange,
                 routingKey: "",
                 basicProperties: null,
                 body: body);
            var queueName = _channel.QueueDeclare().QueueName;
                _channel.QueueBind(queue: queueName,
                              exchange: _exchange,
                              routingKey: "");
                return true;          
        }
        public PublishCommand(ConnectionFactory connectionFactory, string exchange)
        {
            this._exchange = exchange;
            this._factory = connectionFactory;
            this._connection = _factory.CreateConnection();
            this._channel = _connection.CreateModel();


        }

    }
}
