using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Web_Api.Repositories;

namespace Web_Api
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


            services.AddTransient<IApiDochazkaRepository, ApiDochazkaRepository>();
            services.AddSingleton<Publisher>(s => new Publisher(factory, "dochazka.ex", "dochazka.q"));
            services.AddDbContext<DochazkaDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:DbConn"]));
            services.AddDbContext<DochazkaDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:DbConn"]));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
