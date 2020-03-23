using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandHandler;
using EventLibrary;
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
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Uzivatel_Api.Repositories;

namespace Uzivatel_Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Uzivatel Api", Version = "v1" });
            });
            services.AddSwaggerDocument();

            //Description: Kontrola stavu služby
            services.AddHealthChecks()
                .AddCheck("Uzivatel API", () => HealthCheckResult.Healthy())
                .AddSqlServer(connectionString: Configuration["ConnectionString:DbConn"],
                        healthQuery: "SELECT 1;",
                        name: "DB",
                        failureStatus: HealthStatus.Degraded)
                 .AddRabbitMQ(sp => factory);
           
            var exchanges = new List<string>();
            exchanges.Add("uzivatel.ex");

            services.AddTransient<IUzivatelRepository, UzivatelRepository>();
            services.AddSingleton<Publisher>(s => new Publisher(factory, exchanges[0], "uzivatel.q"));
            services.AddDbContext<UzivatelDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:DbConn"]));
            services.AddControllers();

            //Description: Pøihlášení k odbìru zpráv z RabbitMQ
            factory.AutomaticRecoveryEnabled = true;
            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);

            var _connection = factory.CreateConnection();
            //Descripiton: vytvoøení komunikaèního kanálu, pøipojení a definice fronty pro práci se správami
            var _channel = _connection.CreateModel();
            var queueName = _channel.QueueDeclare().QueueName;
            var consumer = services.AddSingleton<ISubscriber>(s => new Subscriber(exchanges, _connection, _channel, queueName)).BuildServiceProvider().GetService<ISubscriber>().Start();
            //Description: Propojení exchange a fronty
            foreach (var ex in exchanges)
            {
                _channel.QueueBind(queue: queueName,
                              exchange: ex,
                              routingKey: "");
            }
            //Description: vytvoøení smìrovaèe pøijatých zpráv ke kterým je služba pøihlášena
            var listener = new Listener(services.BuildServiceProvider().GetService<IUzivatelRepository>());
           
            //Description: Zpracování a odeslání zprávy do smìrovaèe
            consumer.Received += (model, ea) =>
            {
                //-------------Description: Formátování pøijaté zprávy
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                //-------------Description: Odeslání zprávy do smìrovaèe
                
                listener.AddCommand(message);
            };

            HealOnStart(services.BuildServiceProvider().GetService<Publisher>(), exchanges[0]);
        }
        public async Task HealOnStart(Publisher publisher,  string willRecoverOn)
        {
            var _handler = new MessageHandler(publisher);

            var msgTypes = new List<MessageType>();
            msgTypes.Add(MessageType.UzivatelCreated);
            msgTypes.Add(MessageType.UzivatelUpdated);
            msgTypes.Add(MessageType.UzivatelRemoved);
            var evt = new EventServiceReady() { Exchange = willRecoverOn, MessageTypes = msgTypes };
            var msg = new Message()
            {
                Guid = Guid.NewGuid(),
                MessageType = MessageType.HealMe,                  
                Created = DateTime.Now,
                ParentGuid = null,
                Event = await Task.Run(() => JsonConvert.SerializeObject(evt))
            };
            await publisher.Push(JsonConvert.SerializeObject(msg));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHealthChecks("/hc");
            app.UseStaticFiles();
            //Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseOpenApi();
            app.UseSwaggerUi3();
            //Enable middleware to serve swagger - ui(HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Uzivatel Api v1");
            });

            app.UseHttpsRedirection();

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
                endpoints.MapControllerRoute(
     name: "default",
     pattern: "{controller}/{action}/{id?}");
            });
        }
    }
}
