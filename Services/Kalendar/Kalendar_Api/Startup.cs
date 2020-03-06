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
           
            //Description: P�id�n� slu�by popisu metod API
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kalendar Api", Version = "v1" });
            });
            services.AddSwaggerDocument();
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };

            //Description: Kontrola stavu slu�by
            services.AddHealthChecks()
                .AddCheck("API Kalendar", () => HealthCheckResult.Healthy())
                .AddSqlServer(connectionString: Configuration["ConnectionString:DbConn"],
                        healthQuery: "SELECT 1;",
                        name: "DB",
                        failureStatus: HealthStatus.Degraded)
                 .AddRabbitMQ(sp => factory);

            services.AddControllers();

            //Description: Vytvo�en� factory pro RabbitMQ

            var exchanges = new List<string>();

            exchanges.Add(Configuration["ConnectionString:Exchange"]);            
            //Description: Seznam z�jmov�ch exchage            
            exchanges.Add("dochazka.ex");
            exchanges.Add("udalost.ex");
            exchanges.Add("uzivatel.ex");

            //Description: Vytvo�en� reposit��e pro pr�ci s daty
            services.AddTransient<IKalendarRepository, KalendarRepository>();

            //Description: Nastaven� publikov�n� zpr�v
            services.AddSingleton<Publisher>(s => new Publisher(factory, exchanges[0], "kalendar.q"));

            //Description: Vytvo�en� p��stupu k datab�zi
            services.AddDbContext<KalendarDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:DbConn"]));

            //Description: P�ihl�en� k odb�ru zpr�v z RabbitMQ
            factory.AutomaticRecoveryEnabled = true;
            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);
            var _connection = factory.CreateConnection();

            //Descripiton: vytvo�en� komunika�n�ho kan�lu, p�ipojen� a definice fronty pro pr�ci se spr�vami
            var _channel = _connection.CreateModel();
            var queueName = _channel.QueueDeclare().QueueName;
            var consumer = services.AddSingleton<ISubscriber>(s => new Subscriber(exchanges, _connection, _channel, queueName)).BuildServiceProvider()
                    .GetService<ISubscriber>()
                    .Start();
            //Description: Propojen� exchange a fronty
            foreach (var ex in exchanges)
            {
                _channel.QueueBind(queue: queueName,
                              exchange: ex,
                              routingKey: "");
            }
            //Description: vytvo�en� sm�rova�e p�ijat�ch zpr�v ke kter�m je slu�ba p�ihl�ena
            var repository = new ListenerRouter(services.BuildServiceProvider().GetService<IKalendarRepository>());

            //Description: Zpracov�n� a odesl�n� zpr�vy do sm�rova�e
            consumer.Received += (model, ea) =>
            {
                //-------------Description: Form�tov�n� p�ijat� zpr�vy
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);

                //-------------Description: Odesl�n� zpr�vy do sm�rova�e
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

                //Description: Cesta pro Stav slu�by



                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action}/{id?}");
            });
        }
    }
}
