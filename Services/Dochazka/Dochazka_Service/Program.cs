using Dochazka_Service.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Configuration.Json;
using System;
using Udalost_Service;
using System.IO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Dochazka_Service
{
    class Program
    {
         public static IConfiguration Configuration { get; }
        public Program(IConfiguration configuration)
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            
        }
        public void Configure(IApplicationBuilder app)
        {
            
        }
        public void ConfigureServices(IServiceCollection services)
        {
          
            services.AddTransient<IDochazkaRepository, DochazkaRepository>();
            services.AddDbContext<DochazkaDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:DbConn"]));
           
        }

        static void Main(string[] args)
        {
   
            //TODO přesunout samostatně, udělat singleton a nastartovat při spuštění app
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "dochazka.ex", type: ExchangeType.Fanout);

                var queueName = channel.QueueDeclare().QueueName;
                channel.QueueBind(queue: queueName,
                                  exchange: "dochazka.ex",
                                  routingKey: "");

                Console.WriteLine(" [*] Waiting for logs.");

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    //Po přijmutí Eventu vytvořit záznam;
                    var repository = new DochazkaRepository();
                    repository.AddMessage(message);

                    Console.WriteLine(" [x] {0}", message);
                };




                channel.BasicConsume(queue: queueName,
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();

            }
      
    }
    //public static class ApplicationBuilderExtentions
    //{
    //    //the simplest way to store a single long-living object, just for example.
    //    private static RabbitListener _listener { get; set; }

    //    public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
    //    {
    //        _listener = app.ApplicationServices.GetService<RabbitListener>();

    //        var lifetime = app.ApplicationServices.GetService<IApplicationLifetime>();

    //        lifetime.ApplicationStarted.Register(OnStarted);

    //        //press Ctrl+C to reproduce if your app runs in Kestrel as a console app
    //        lifetime.ApplicationStopping.Register(OnStopping);

    //        return app;
    //    }

    //    private static void OnStarted()
    //    {
    //        _listener.Register();
    //    }

    //    private static void OnStopping()
    //    {
    //        _listener.Deregister();
    //    }
    //}
    //public class RabbitListener
    //{
    //    ConnectionFactory factory { get; set; }
    //    IConnection connection { get; set; }
    //    IModel channel { get; set; }

    //    public void Register()
    //    {
    //        channel.QueueDeclare(queue: "dochazka.q", durable: false, exclusive: false, autoDelete: false, arguments: null);

    //        var consumer = new EventingBasicConsumer(channel);
    //        consumer.Received += (model, ea) =>
    //        {
    //            var body = ea.Body;
    //            var message = Encoding.UTF8.GetString(body);
    //            int m = 0;
    //        };
    //        channel.BasicConsume(queue: "dochazka.q", autoAck: true, consumer: consumer);
    //    }

    //    public void Deregister()
    //    {
    //        this.connection.Close();
    //    }

    //    public RabbitListener()
    //    {
    //        this.factory = new ConnectionFactory() { HostName = "rabbitmq" };
    //        this.connection = factory.CreateConnection();
    //        this.channel = connection.CreateModel();


    //    }
    }
}
