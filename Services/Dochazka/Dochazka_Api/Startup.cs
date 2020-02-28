using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandHandler;
using Dochazka_Api.Repositories;
using HealthChecks.RabbitMQ;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;

namespace Dochazka_Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //Description: Pøidání služby popisu metod API
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dochazka Api", Version = "v1" });
            });
            services.AddSwaggerDocument();
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };

            //Description: Kontrola stavu služby
            services.AddHealthChecks()
                .AddCheck("API Dochazka", () => HealthCheckResult.Healthy())
                .AddSqlServer(connectionString: Configuration["ConnectionString:DbConn"],
                        healthQuery: "SELECT 1;",
                        name: "Database",
                        failureStatus: HealthStatus.Degraded)
                 .AddRabbitMQ(sp => factory);


            //services.AddHealthChecksUI(setupSettings: setup =>
            //{
            //    setup.AddHealthCheckEndpoint("Dochazka Api", "http://dochazkaapi/healthcheck");
            //});

            services.AddControllers();

            //Description: Vytvoøení factory pro RabbitMQ

            var exchanges = new List<string>();

            //exchanges.Add(Configuration.GetValue<string>("Setting:Exchange"));
            //Description: Seznam zájmových exchage
            exchanges.Add("dochazka.ex");

            //Description: Vytvoøení repositáøe pro práci s daty
            services.AddTransient<IDochazkaRepository, DochazkaRepository>();

            //Description: Nastavení publikování zpráv
            services.AddSingleton<Publisher>(s => new Publisher(factory, exchanges[0], "dochazka.q"));

            //Description: Vytvoøení pøístupu k databázi
            services.AddDbContext<DochazkaDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:DbConn"]));

            //Description: Pøihlášení k odbìru zpráv z RabbitMQ
            factory.AutomaticRecoveryEnabled = true;
            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);
            var _connection = factory.CreateConnection();

            //Descripiton: vytvoøení komunikaèního kanálu, pøipojení a definice fronty pro práci se správami
            var _channel = _connection.CreateModel();
            var queueName = _channel.QueueDeclare().QueueName;
            var consumer = services.AddSingleton<ISubscriber>(s => new Subscriber(exchanges, _connection, _channel, queueName)).BuildServiceProvider()
                    .GetService<ISubscriber>()
                    .Start();
            //Description: Propojení exchange a fronty
            foreach (var ex in exchanges)
            {
                _channel.QueueBind(queue: queueName,
                              exchange: ex,
                              routingKey: "");
            }
            //Description: vytvoøení smìrovaèe pøijatých zpráv ke kterým je služba pøihlášena
            var repository = new ListenerRouter(services.BuildServiceProvider().GetService<IDochazkaRepository>());

            //Description: Zpracování a odeslání zprávy do smìrovaèe
            consumer.Received += (model, ea) =>
            {
                //-------------Description: Formátování pøijaté zprávy
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                //-------------Description: Odeslání zprávy do smìrovaèe
                repository.AddCommand(message);
            };
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseStaticFiles();

            app.UseOpenApi();

            app.UseSwaggerUi3();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dochazka Api v1");
            });

            app.UseHttpsRedirection();



            app.UseRouting();

            app.UseHealthChecks("/healthcheck", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            //app.UseHealthChecksUI();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                //Description: Cesta pro Stav služby



                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action}/{id?}");
            });
        }
    }
}
