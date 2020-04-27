using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

using Uzivatel_Api;
using Uzivatel_Api.Repositories;

namespace Uzivatel_Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        private IConfiguration Configuration { get; }
        private IConnection Connection { get; set; }
        public void ConfigureServices(IServiceCollection services)
        {
            //Description: Pøidání objektu obsluhy operací a databáze
            services.AddTransient<IRepository, Repository>();
            //Description: Vytvoøení DbContextu a pøipojení do databáze. ConnectionString v appsettings.json
            services.AddDbContext<ServiceDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:DbConn"]));
            //Description: Generování popisu metod API. Název modulu v appsettings.json
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = Configuration["Modul:Name"], Version = "v1" });
            });        
            //Description: Pøidání souboru s popisem API
            services.AddSwaggerDocument();
            //Description: Vytvoøení kontrolerù pro metody API
            services.AddControllers();
            //Description: Vytvoøení kontroly stavu modulu a databáze a message brokeru
            services.AddHealthChecks()
               .AddCheck(Configuration["Modul:Name"], () => HealthCheckResult.Healthy())
               .AddSqlServer(connectionString: Configuration["ConnectionString:DbConn"],
                       healthQuery: "SELECT 1;",
                       name: "DB",
                       failureStatus: HealthStatus.Degraded).AddRabbitMQ(sp => Connection);
            //Description: Navání spojení s Message Broker
            MessageBrokerConnection(services);
        }
        public async void MessageBrokerConnection(IServiceCollection services)
        {
            //Description: Seznam zájmových Exchange ke konzumaci událostí
            var exchanges = Configuration.GetSection("RbSetting:Subscription").Get<List<string>>();   
            //Description: Nastavení pøipojení k MB
            var factory = new ConnectionFactory() { HostName = Configuration["ConnectionString:RbConn"] };
            //Description: Nastavení cyklické kontroly stavu, automatického obnovení a intervalu
            factory.RequestedHeartbeat = 60;
            factory.AutomaticRecoveryEnabled = true;
            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(15);

            //Description: Vytvoøení objektu pro Poskytnutí metod Publikace událostí
            //Description: Pøidìlení základního Exchange a fronty servisu.
            services.AddSingleton<Publisher>(s => new Publisher(factory, Configuration["RbSetting:Exchange"], Configuration["RbSetting:Queue"]));
          
            //Description: Nastavení politiky reakce na vyjímku pøi nedostupnosti
            //Description: v pøípadì vyjímky opakovat 5krát, každých 10 vteøin
            var retryPolicy = Policy
                .Handle<BrokerUnreachableException>()
                .WaitAndRetryAsync(5, i => TimeSpan.FromSeconds(10));
            await retryPolicy.ExecuteAsync(async () =>
            {
                await Task.Run(() => {
                    try
                    {
                        //Description: Navázání pøipojení s Message Broker
                        Connection = factory.CreateConnection();
                        //Description: Vytvoøení kanálu na MB pro pøipojení servisu
                        var _channel = Connection.CreateModel();
                        //Description: Deklarace fronty pro servis
                        var queueName = _channel.QueueDeclare().QueueName;
                        //Description: Vytvoøení objektu singleton Konzumaci naslouchání aktivity MB a Konzumace událostí         
                        var consumer = services.AddSingleton<ISubscriber>(s => new Subscriber(exchanges, Connection, _channel, queueName))
                            .BuildServiceProvider().GetService<ISubscriber>().Start();
                        //Description: Návázání spojení na zájmové Exchange
                        foreach (var ex in exchanges)
                        {
                            _channel.QueueBind(queue: queueName,
                                          exchange: ex,
                                          routingKey: "");
                        }
                        //Description: Vytvoøení objektu s metodami reakce na konzumované události
                        var listener = new Listener(services.BuildServiceProvider().GetService<IRepository>());
                        //Description: Zpracování konzumované zprávy z Message Broker
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body);
                            //Description: Pøedání zprávy pro nasmìrování a zpracování.
                            listener.AddCommand(message);
                        };
                    }
                    catch (Exception)
                    {     
                        //ToDo: Prostor pro logování vyjímky a další reakce
                    }

                });
            });
          
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //Description: Vystavení adresy pro kontrolu stavu
            app.UseHealthChecks("/hc");
            app.UseStaticFiles();
            //Description: Aktivace popisu API, Vystavení popisu na rozhraní
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Configuration["Modul:Name"]} v1");
            });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            //Description: Vystavení výsledkù kontroly stavu na rozhraní
            app.UseHealthChecks("/healthcheck", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            //Description: Pøedpis pro smìrování pøíchozích požadavkù na Controller
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
