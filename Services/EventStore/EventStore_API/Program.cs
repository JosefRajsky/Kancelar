using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EventStore_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

//static void Main(string[] args)
//{
//    try
//    {
//        IConfiguration config = new ConfigurationBuilder()
//            .AddJsonFile("appsettings.json", true, true)
//            .Build();
//        var exchanges = new List<string>();
//        exchanges.Add(config.GetValue<string>("Setting:Exchange"));

//        var factory = new ConnectionFactory() { HostName = "rabbitmq" };
//        var _factory = factory;
//        _factory.AutomaticRecoveryEnabled = true;
//        _factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);
//        var _connection = _factory.CreateConnection();
//        var _channel = _connection.CreateModel();
//        var queueName = _channel.QueueDeclare().QueueName;

//        var services = new ServiceCollection();
//        var serviceProvider = services.BuildServiceProvider();

//        var consumer = new ServiceCollection()
//            .AddSingleton<ISubscriber>(s => new Subscriber(exchanges, _connection, _channel, queueName))
//           .BuildServiceProvider()
//            .GetService<ISubscriber>()
//            .Start();
//        foreach (var ex in exchanges)
//        {
//            _channel.QueueBind(queue: queueName,
//                          exchange: ex,
//                          routingKey: "");
//        }
//        var repository = new EventStoreRepository(config.GetValue<string>("Setting:ConnectionString"));
//        consumer.Received += (model, ea) =>
//        {
//            var body = ea.Body;
//            var message = Encoding.UTF8.GetString(body);

//            repository.AddMessageAsync(message).Wait();
//        };
//    }
//    catch (Exception exception)
//    {
//        throw exception;
//    }
//}
