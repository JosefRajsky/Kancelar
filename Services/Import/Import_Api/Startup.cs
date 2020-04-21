using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandHandler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;

namespace Import_Api
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Import Servis", Version = "v1" });
            });
            services.AddSwaggerDocument();

            services.AddControllers();
            services.AddHealthChecks().AddCheck("Import Servis", () => HealthCheckResult.Healthy());
            
            ConnectMessageBroker(services);


        }
        public async void ConnectMessageBroker(IServiceCollection services)
        {
            await Task.Run(() =>
            {
                var exchanges = new List<string>();
                exchanges.Add("import.ex");
                var factory = new ConnectionFactory() { HostName = Configuration["ConnectionString:RbConn"] };
                factory.AutomaticRecoveryEnabled = true;
                factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);

                services.AddSingleton<Publisher>(s => new Publisher(factory, exchanges[0], "import.q"));
                var _connection = factory.CreateConnection();
                var _channel = _connection.CreateModel();
                //_channel.ExchangeDeclare(exchange: exchanges[0], type: ExchangeType.Fanout, false, false, args);
                var queueName = _channel.QueueDeclare().QueueName;

                //foreach (var ex in exchanges)
                //{
                //    _channel.QueueBind(queue: queueName,
                //                  exchange: ex,
                //                  routingKey: "");
                //}
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
            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Import servis v1");
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

