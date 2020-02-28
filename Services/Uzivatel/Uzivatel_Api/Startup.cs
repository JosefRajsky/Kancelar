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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using Uzivatel_Api.Repositories;

namespace Uzivatel_Api
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
            var factory = new ConnectionFactory() { HostName = "rabbitmq" };
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Uzivatel Api", Version = "v1" });
            });
            services.AddSwaggerDocument();

            //Description: Kontrola stavu služby
            services.AddHealthChecks()
                .AddCheck("Servis", () => HealthCheckResult.Healthy())
                .AddSqlServer(connectionString: Configuration["ConnectionString:DbConn"],
                        healthQuery: "SELECT 1;",
                        name: "DB",
                        failureStatus: HealthStatus.Degraded)
                 .AddRabbitMQ(sp => factory);



            services.AddTransient<IUzivatelRepository, UzivatelRepository>();
            services.AddSingleton<Publisher>(s => new Publisher(factory, "uzivatel.ex", "uzivatel.q"));
            services.AddDbContext<UzivatelDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:DbConn"]));
            services.AddControllers();
            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Uzivatel Api v1");
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
