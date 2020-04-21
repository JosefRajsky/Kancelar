using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandHandler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using Template.Repositories;

namespace Template
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
            services.AddTransient<IRepository, Repository>();
            services.AddDbContext<TemplateDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:DbConn"]));
            services.AddControllers();
            //Description: Kontrola stavu slu�by
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Template Api", Version = "v1" });
            });
          
            services.AddSwaggerDocument();
            services.AddHealthChecks()
                .AddCheck("Template API", () => HealthCheckResult.Healthy())
                .AddSqlServer(connectionString: Configuration["ConnectionString:DbConn"],
                        healthQuery: "SELECT 1;",
                        name: "DB",
                        failureStatus: HealthStatus.Degraded);
            ConnectMessageBroker(services);
        }
        public async void ConnectMessageBroker(IServiceCollection services)
        {
            await Task.Run(() =>
            {
                var exchanges = new List<string>();
                exchanges.Add(Configuration["RbSetting:Exchange"]);
                var factory = new ConnectionFactory() { HostName = Configuration["RbSetting:RbConn"] };          
                services.AddSingleton<Publisher>(s => new Publisher(factory, Configuration["RbSetting:Exchange"], "Template.q"));               
                //Description: P�ihl�en� k odb�ru zpr�v z RabbitMQ
                factory.AutomaticRecoveryEnabled = true;
                factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);
                var _connection = factory.CreateConnection();
                //Descripiton: vytvo�en� komunika�n�ho kan�lu, p�ipojen� a definice fronty pro pr�ci se spr�vami
                var _channel = _connection.CreateModel();
                var queueName = _channel.QueueDeclare().QueueName;
                var consumer = services.AddSingleton<ISubscriber>(s => new Subscriber(exchanges, _connection, _channel, queueName)).BuildServiceProvider().GetService<ISubscriber>().Start();
                //Description: Propojen� exchange a fronty
                foreach (var ex in exchanges)
                {
                    _channel.QueueBind(queue: queueName,
                                  exchange: ex,
                                  routingKey: "");
                }
                //Description: vytvo�en� sm�rova�e p�ijat�ch zpr�v ke kter�m je slu�ba p�ihl�ena
                var listener = new Listener(services.BuildServiceProvider().GetService<IRepository>());
                //Description: Zpracov�n� a odesl�n� zpr�vy do sm�rova�e
                consumer.Received += (model, ea) =>
                {
                    //-------------Description: Form�tov�n� p�ijat� zpr�vy
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    //-------------Description: Odesl�n� zpr�vy do sm�rova�e
                    listener.AddCommand(message);
                };
                services.AddHealthChecks().AddRabbitMQ(sp => _connection);
            });            
        }

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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Temp Api v1");
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
