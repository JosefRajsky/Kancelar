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
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Pritomnost_Api.Repositories;
using RabbitMQ.Client;

namespace Pritomnost_Api
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


            var exchanges = new List<string>();
            exchanges.Add("pritomnost.ex");
            exchanges.Add("dochazka.ex");
            exchanges.Add("udalost.ex");
            exchanges.Add("uzivatel.ex");

            var factory = new ConnectionFactory() { HostName = Configuration["ConnectionString:RbConn"] };
            services.AddTransient<IRepository, Repository>();
            services.AddSingleton<Publisher>(s => new Publisher(factory, exchanges[0], "pritomnost.q"));
            services.AddDbContext<PritomnostDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:DbConn"]));
            services.AddControllers();
            factory.AutomaticRecoveryEnabled = true;
            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);
            var _connection = factory.CreateConnection();
            var _channel = _connection.CreateModel();
            var queueName = _channel.QueueDeclare().QueueName;
            var consumer = services.AddSingleton<ISubscriber>(s => new Subscriber(exchanges, _connection, _channel, queueName)).BuildServiceProvider().GetService<ISubscriber>().Start();
            foreach (var ex in exchanges)
            {
                _channel.QueueBind(queue: queueName,
                              exchange: ex,
                              routingKey: "");
            }
            var repository = new Listener(services.BuildServiceProvider().GetService<IRepository>());
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                repository.AddCommand(message);
            };

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pritomnost Api", Version = "v1" });
            });
            services.AddSwaggerDocument();
            services.AddControllers();
            services.AddHealthChecks()
                .AddCheck("API Pritomnost", () => HealthCheckResult.Healthy())
                .AddSqlServer(connectionString: Configuration["ConnectionString:DbConn"],
                        healthQuery: "SELECT 1;",
                        name: "DB",
                        failureStatus: HealthStatus.Degraded)
                 .AddRabbitMQ(sp => _connection);

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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pritomnost Api v1");
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
