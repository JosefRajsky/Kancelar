using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandHandler;
using EventStore_Api;
using EventStore_Api.Repositories;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace EventStore_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public IConnection _connection { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
                var factory = new ConnectionFactory() { HostName = "rabbitmq" };
                var _factory = factory;
                _factory.AutomaticRecoveryEnabled = true;
                _factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);
            
                try
                {
                _connection = _factory.CreateConnection();
                 }
                catch (BrokerUnreachableException e)
                {
                Thread.Sleep(5000);
                 _connection = _factory.CreateConnection();
                }

          
                var _channel = _connection.CreateModel();
                var queueName = _channel.QueueDeclare().QueueName;

                var exchanges = new List<string>();
                exchanges.Add(Configuration.GetValue<string>("Setting:Exchange"));

                services.AddTransient<IEventStoreRepository, EventStoreRepository>()
                        .AddDbContext<EventStoreDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:DbConn"]))
                        .AddControllers();
                var consumer = services.AddSingleton<ISubscriber>(s => new Subscriber(exchanges, _connection, _channel, queueName))
                        .BuildServiceProvider()
                        .GetService<ISubscriber>()
                        .Start();

                //Description: Kontrola stavu služby
                services.AddHealthChecks()
                     .AddCheck("Event Store", () => HealthCheckResult.Healthy())
                     .AddSqlServer(connectionString: Configuration["ConnectionString:DbConn"],
                             healthQuery: "SELECT 1;",
                             name: "DB",
                             failureStatus: HealthStatus.Degraded)
                      .AddRabbitMQ(sp => factory);


                foreach (var ex in exchanges)
                {
                    _channel.QueueBind(queue: queueName,
                                  exchange: ex,
                                  routingKey: "");
                }
                var repository = new EventStoreRepository(Configuration["ConnectionString:DbConn"]);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);

                    repository.AddMessageAsync(message).Wait();

                };
         
            
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseHealthChecks("/healthcheck", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
