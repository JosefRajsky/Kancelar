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
            //Description: P�id�n� objektu obsluhy operac� a datab�ze
            services.AddTransient<IRepository, Repository>();
            //Description: Vytvo�en� DbContextu a p�ipojen� do datab�ze. ConnectionString v appsettings.json
            services.AddDbContext<ServiceDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:DbConn"]));
            //Description: Generov�n� popisu metod API. N�zev modulu v appsettings.json
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = Configuration["Modul:Name"], Version = "v1" });
            });        
            //Description: P�id�n� souboru s popisem API
            services.AddSwaggerDocument();
            //Description: Vytvo�en� kontroler� pro metody API
            services.AddControllers();
            //Description: Vytvo�en� kontroly stavu modulu a datab�ze a message brokeru
            services.AddHealthChecks()
               .AddCheck(Configuration["Modul:Name"], () => HealthCheckResult.Healthy())
               .AddSqlServer(connectionString: Configuration["ConnectionString:DbConn"],
                       healthQuery: "SELECT 1;",
                       name: "DB",
                       failureStatus: HealthStatus.Degraded).AddRabbitMQ(sp => Connection);
            //Description: Nav�n� spojen� s Message Broker
            MessageBrokerConnection(services);
        }
        public async void MessageBrokerConnection(IServiceCollection services)
        {
            //Description: Seznam z�jmov�ch Exchange ke konzumaci ud�lost�
            var exchanges = Configuration.GetSection("RbSetting:Subscription").Get<List<string>>();   
            //Description: Nastaven� p�ipojen� k MB
            var factory = new ConnectionFactory() { HostName = Configuration["ConnectionString:RbConn"] };
            //Description: Nastaven� cyklick� kontroly stavu, automatick�ho obnoven� a intervalu
            factory.RequestedHeartbeat = 60;
            factory.AutomaticRecoveryEnabled = true;
            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(15);

            //Description: Vytvo�en� objektu pro Poskytnut� metod Publikace ud�lost�
            //Description: P�id�len� z�kladn�ho Exchange a fronty servisu.
            services.AddSingleton<Publisher>(s => new Publisher(factory, Configuration["RbSetting:Exchange"], Configuration["RbSetting:Queue"]));
          
            //Description: Nastaven� politiky reakce na vyj�mku p�i nedostupnosti
            //Description: v p��pad� vyj�mky opakovat 5kr�t, ka�d�ch 10 vte�in
            var retryPolicy = Policy
                .Handle<BrokerUnreachableException>()
                .WaitAndRetryAsync(5, i => TimeSpan.FromSeconds(10));
            await retryPolicy.ExecuteAsync(async () =>
            {
                await Task.Run(() => {
                    try
                    {
                        //Description: Nav�z�n� p�ipojen� s Message Broker
                        Connection = factory.CreateConnection();
                        //Description: Vytvo�en� kan�lu na MB pro p�ipojen� servisu
                        var _channel = Connection.CreateModel();
                        //Description: Deklarace fronty pro servis
                        var queueName = _channel.QueueDeclare().QueueName;
                        //Description: Vytvo�en� objektu singleton Konzumaci naslouch�n� aktivity MB a Konzumace ud�lost�         
                        var consumer = services.AddSingleton<ISubscriber>(s => new Subscriber(exchanges, Connection, _channel, queueName))
                            .BuildServiceProvider().GetService<ISubscriber>().Start();
                        //Description: N�v�z�n� spojen� na z�jmov� Exchange
                        foreach (var ex in exchanges)
                        {
                            _channel.QueueBind(queue: queueName,
                                          exchange: ex,
                                          routingKey: "");
                        }
                        //Description: Vytvo�en� objektu s metodami reakce na konzumovan� ud�losti
                        var listener = new Listener(services.BuildServiceProvider().GetService<IRepository>());
                        //Description: Zpracov�n� konzumovan� zpr�vy z Message Broker
                        consumer.Received += (model, ea) =>
                        {
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body);
                            //Description: P�ed�n� zpr�vy pro nasm�rov�n� a zpracov�n�.
                            listener.AddCommand(message);
                        };
                    }
                    catch (Exception)
                    {     
                        //ToDo: Prostor pro logov�n� vyj�mky a dal�� reakce
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
            //Description: Vystaven� adresy pro kontrolu stavu
            app.UseHealthChecks("/hc");
            app.UseStaticFiles();
            //Description: Aktivace popisu API, Vystaven� popisu na rozhran�
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Configuration["Modul:Name"]} v1");
            });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            //Description: Vystaven� v�sledk� kontroly stavu na rozhran�
            app.UseHealthChecks("/healthcheck", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            //Description: P�edpis pro sm�rov�n� p��choz�ch po�adavk� na Controller
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
