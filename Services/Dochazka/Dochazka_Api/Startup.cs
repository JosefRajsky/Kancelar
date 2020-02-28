using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandHandler;
using Consul;
using Dochazka_Api.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;

namespace Dochazka_Api
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dochazka Api", Version = "v1" });
            });
            services.AddSwaggerDocument();

            services.AddTransient<IDochazkaRepository, DochazkaRepository>();
            services.AddSingleton<Publisher>(s => new Publisher(factory, "dochazka.ex","dochazka.q"));
            services.AddDbContext<DochazkaDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:DbConn"]));    
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        //public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.AspNetCore.Hosting.IApplicationLifetime lifetime)
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            //Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseOpenApi();
            app.UseSwaggerUi3();
            //Enable middleware to serve swagger - ui(HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Dochazka Api v1");
            });

            //// Retrieve Consul client from DI
            //var consulClient = app.ApplicationServices
            //                    .GetRequiredService<IConsulClient>();
            //var consulConfig = app.ApplicationServices
            //                    .GetRequiredService<IOptions<ConsulConfig>>();

            //// Get server address information
            //var features = app.Properties["server.Features"] as FeatureCollection;
            //var addresses = features.Get<IServerAddressesFeature>();
            //var address = addresses.Addresses.First();
            //var uri = new Uri(address);

            //// Register service with consul
            //var registration = new AgentServiceRegistration()
            //{
            //    ID = $"{consulConfig.Value.ServiceID}-{uri.Port}",
            //    Name = consulConfig.Value.ServiceName,
            //    Address = $"{uri.Scheme}://{uri.Host}",
            //    Port = uri.Port,
            //    Tags = new[] { "Students", "Courses", "School" }
            //};
            //consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            //consulClient.Agent.ServiceRegister(registration).Wait();

            //lifetime.ApplicationStopping.Register(() => {
            //    consulClient.Agent.ServiceDeregister(registration.ID).Wait();
            //});


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

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
