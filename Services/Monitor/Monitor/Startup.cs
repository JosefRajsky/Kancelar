using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HealthChecks.RabbitMQ;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Monitor
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
            services.AddControllers();            
            services.AddHealthChecks().AddCheck("Monitor", () => HealthCheckResult.Healthy());
            services.AddHealthChecksUI(setupSettings: setup =>
            {
                
                setup.AddHealthCheckEndpoint("Monitor", "http://monitor/healthcheck");
                setup.AddHealthCheckEndpoint("Dochazka", "http://dochazkaapi/healthcheck");
                setup.AddHealthCheckEndpoint("Uzivatel", "http://uzivatelapi/healthcheck");
                setup.AddHealthCheckEndpoint("Eventstore", "http://eventstore/healthcheck");
                setup.AddHealthCheckEndpoint("Kalendar", "http://kalendarapi/healthcheck");
                setup.AddHealthCheckEndpoint("Pritomnost", "http://pritomnostapi/healthcheck");               
                setup.AddHealthCheckEndpoint("Aktivita", "http://aktivitaapi/healthcheck");
                setup.AddHealthCheckEndpoint("Cinnost", "http://cinnostapi/healthcheck");
                setup.AddHealthCheckEndpoint("MailSender", "http://mailsenderapi/healthcheck");
                setup.AddHealthCheckEndpoint("Mzdy", "http://mzdyapi/healthcheck");
                setup.AddHealthCheckEndpoint("Nastaveni", "http://nastaveniapi/healthcheck");
                setup.AddHealthCheckEndpoint("Opravneni", "http://opravneniapi/healthcheck");
                setup.AddHealthCheckEndpoint("Soucast", "http://soucastapi/healthcheck");
                setup.AddHealthCheckEndpoint("Struktura", "http://strukturaapi/healthcheck");
                setup.AddHealthCheckEndpoint("Ukol", "http://ukolapi/healthcheck");
                setup.AddHealthCheckEndpoint("Vykaz", "http://vykazapi/healthcheck");
                setup.AddHealthCheckEndpoint("Transfer", "http://transferapi/healthcheck");
            });
            MessageBrokerConnection(services);
        services.AddHealthChecks().AddRabbitMQ(sp => Connection);
        }
        public async void MessageBrokerConnection(IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var factory = new ConnectionFactory() { HostName = Configuration["ConnectionString:RbConn"] };
            factory.RequestedHeartbeat = 60;
            factory.AutomaticRecoveryEnabled = true;
            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(15);
            
            var retryPolicy = Policy.Handle<BrokerUnreachableException>().WaitAndRetryAsync(5, i => TimeSpan.FromSeconds(10));
            await retryPolicy.ExecuteAsync(async () =>
            {
                await Task.Run(() => {
                    try
                    {
                        Connection = factory.CreateConnection();
                    }
                    catch (Exception)
                    {

                        
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
            app.UseRouting();

            app.UseAuthorization();
            app.UseHealthChecks("/healthcheck", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.UseHealthChecksUI();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
