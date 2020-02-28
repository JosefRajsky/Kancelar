using CommandHandler;
using EventStore_Service.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventStore_Service
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {                
                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();               
                var exchanges = new List<string>();
                exchanges.Add(config.GetValue<string>("Setting:Exchange"));

                var factory = new ConnectionFactory() { HostName = "rabbitmq" };
                var _factory = factory;
                _factory.AutomaticRecoveryEnabled = true;
                _factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);
                var _connection = _factory.CreateConnection();
                var _channel = _connection.CreateModel();
                var queueName = _channel.QueueDeclare().QueueName;
    

                var consumer = new ServiceCollection()
                    .AddSingleton<ISubscriber>(s => new Subscriber(exchanges,_connection,_channel,queueName))
                    .BuildServiceProvider()
                    .GetService<ISubscriber>()
                    .Start();
                foreach (var ex in exchanges)
                {
                    _channel.QueueBind(queue: queueName,
                                  exchange: ex,
                                  routingKey: "");
                }
                var repository = new EventStoreRepository(config.GetValue<string>("Setting:ConnectionString"));
                consumer.Received += (model, ea) =>
                {                   
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);                   
                                   
                    repository.AddMessageAsync(message).Wait();
                };
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
