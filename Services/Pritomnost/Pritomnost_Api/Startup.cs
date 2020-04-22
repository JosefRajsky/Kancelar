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
using Polly;
using Pritomnost_Api.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Pritomnost_Api
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public IConnection Connection { get; set; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IRepository, Repository>();
            services.AddDbContext<ServiceDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:DbConn"]));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = Configuration["Modul:Name"], Version = "v1" });
            });
            services.AddSwaggerDocument();
            services.AddControllers();
            services.AddHealthChecks()
               .AddCheck(Configuration["Modul:Name"], () => HealthCheckResult.Healthy())
               .AddSqlServer(connectionString: Configuration["ConnectionString:DbConn"],
                       healthQuery: "SELECT 1;",
                       name: "DB",
                       failureStatus: HealthStatus.Degraded).AddRabbitMQ(sp => Connection);
            MessageBrokerConnection(services);
        }
        public async void MessageBrokerConnection(IServiceCollection services)
        {
            var exchanges = Configuration.GetSection("RbSetting:Subscription").Get<List<string>>();
            var factory = new ConnectionFactory() { HostName = Configuration["ConnectionString:RbConn"] };
            factory.RequestedHeartbeat = 60;
            factory.AutomaticRecoveryEnabled = true;
            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(15);
            services.AddSingleton<Publisher>(s => new Publisher(factory, Configuration["RbSetting:Exchange"], Configuration["RbSetting:Queue"]));
            var retryPolicy = Policy.Handle<BrokerUnreachableException>().WaitAndRetryAsync(5, i => TimeSpan.FromSeconds(10));
            await retryPolicy.ExecuteAsync(async () =>
            {
                await Task.Run(() => { Connection = factory.CreateConnection(); });
            });
            var _channel = Connection.CreateModel();
            var queueName = _channel.QueueDeclare().QueueName;
            var consumer = services.AddSingleton<ISubscriber>(s => new Subscriber(exchanges, Connection, _channel, queueName)).BuildServiceProvider().GetService<ISubscriber>().Start();
            foreach (var ex in exchanges)
            {
                _channel.QueueBind(queue: queueName,
                              exchange: ex,
                              routingKey: "");
            }
            var listener = new Listener(services.BuildServiceProvider().GetService<IRepository>());
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                listener.AddCommand(message);
            };
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Configuration["Modul:Name"]} v1");
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
