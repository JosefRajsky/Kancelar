using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandHandler;
using HealthChecks.UI.Client;
using Kalendar_Api.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;

namespace Kalendar_Api
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kalendar Api", Version = "v1" });
            });
            services.AddSwaggerDocument();
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };

            //Description: Kontrola stavu služby
            services.AddHealthChecks()
                .AddCheck("API Kalendar", () => HealthCheckResult.Healthy())
                .AddSqlServer(connectionString: Configuration["ConnectionString:DbConn"],
                        healthQuery: "SELECT 1;",
                        name: "DB",
                        failureStatus: HealthStatus.Degraded)
                 .AddRabbitMQ(sp => factory);

            services.AddControllers();

            //Description: Vytvoøení factory pro RabbitMQ

            var exchanges = new List<string>();

            exchanges.Add(Configuration["ConnectionString:Exchange"]);            
            //Description: Seznam zájmových exchage            
            exchanges.Add("dochazka.ex");
            exchanges.Add("udalost.ex");
            exchanges.Add("uzivatel.ex");

            //Description: Vytvoøení repositáøe pro práci s daty
            services.AddTransient<IKalendarRepository, KalendarRepository>();

            //Description: Nastavení publikování zpráv
            services.AddSingleton<Publisher>(s => new Publisher(factory, exchanges[0], "kalendar.q"));

            //Description: Vytvoøení pøístupu k databázi
            services.AddDbContext<KalendarDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:DbConn"]));

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
            var repository = new ListenerRouter(services.BuildServiceProvider().GetService<IKalendarRepository>());

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
            var supportedCultures = new[] { new CultureInfo("cs-CS") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("cs-CS"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

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
