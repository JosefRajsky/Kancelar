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

using RabbitMQ.Client;
namespace Monitor
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
            services.AddControllers();

            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            services.AddHealthChecks().AddCheck("Monitor", () => HealthCheckResult.Healthy());
            services.AddHealthChecks().AddRabbitMQ(sp => factory);    
         
            services.AddHealthChecksUI(setupSettings: setup =>
            {
                setup.AddHealthCheckEndpoint("Template", "http://template/healthcheck");
                setup.AddHealthCheckEndpoint("Monitor", "http://monitor/healthcheck");
                setup.AddHealthCheckEndpoint("Dochazka", "http://dochazkaapi/healthcheck");           
                setup.AddHealthCheckEndpoint("Udalost", "http://udalostapi/healthcheck");
                setup.AddHealthCheckEndpoint("Uzivatel", "http://uzivatelapi/healthcheck");
                setup.AddHealthCheckEndpoint("Eventstore", "http://eventstore/healthcheck");
                setup.AddHealthCheckEndpoint("Kalendar", "http://kalendarapi/healthcheck");
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
