using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CommandHandler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Udalost_Api.Repositories;

namespace Udalost_Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
              var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            services.AddTransient<IUdalostRepository, UdalostRepository>();
            services.AddSingleton<Publisher>(s => new Publisher(factory, "udalost.ex","udalost.q"));
            services.AddDbContext<UdalostDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:DbConn"]));
            services.AddControllers();
            services.AddHealthChecks();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Udalost Api", Version = "v1" });
            });
            var exchanges = new List<string>();
            exchanges.Add("udalost.ex");
            exchanges.Add("dochazka.ex");

         
            factory.AutomaticRecoveryEnabled = true;
            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);

            

            //Description: Kontrola stavu služby
            services.AddHealthChecks()
                .AddCheck("API Udalost", () => HealthCheckResult.Healthy())
                .AddSqlServer(connectionString: Configuration["ConnectionString:DbConn"],
                        healthQuery: "SELECT 1;",
                        name: "DB",
                        failureStatus: HealthStatus.Degraded)
                 .AddRabbitMQ(sp => factory);

            var _connection = factory.CreateConnection();
            var _channel = _connection.CreateModel();
            var queueName = _channel.QueueDeclare().QueueName;
            var consumer = services.AddSingleton<ISubscriber>(s => new Subscriber(exchanges, _connection, _channel,queueName)).BuildServiceProvider()
                    .GetService<ISubscriber>()
                    .Start();
            foreach (var ex in exchanges)
            {
                _channel.QueueBind(queue: queueName,
                              exchange: ex,
                              routingKey: "");
            }
            var repository = new UdalostServiceRepository(services.BuildServiceProvider().GetService<IUdalostRepository>());
            consumer.Received += (model, ea) =>
            {
                //-------------Description: Formátování pøijaté zprávy
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                //-------------Description: Vytvoøení repositáøe pro pøístup k entitám služby.

            
                //-------------Description: Odeslání zprávy do repositáøe
                repository.AddCommand(message);
            };

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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dochazka Api v1");
            });

            app.UseRouting();
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
                  pattern: "{controller}/{action}/{id}");
            });
        }
    }
}
