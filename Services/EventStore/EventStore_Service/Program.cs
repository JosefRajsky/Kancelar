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
                var consumer = new ServiceCollection()
                    .AddSingleton<ISubscriber>(s => new Subscriber(new ConnectionFactory() { HostName = "rabbitmq" }, exchanges))
                    .BuildServiceProvider()
                    .GetService<ISubscriber>()
                    .Start();
                consumer.Received += (model, ea) =>
                {                   
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);                   
                    var repository = new EventStoreRepository(config.GetValue<string>("Setting:ConnectionString"));                 
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
