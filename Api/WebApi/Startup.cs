using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.IO;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Ocelot.Provider.Kubernetes;
using OcelotSwagger.Extensions;
using OcelotSwagger.Configuration;
using Ocelot.Administration;
using IdentityServer4.AccessTokenValidation;
using Microsoft.OpenApi.Models;
using Ocelot.Provider.Consul;

namespace WebApi
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup()
        {

            _config = new ConfigurationBuilder()                
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("ocelot.json", false, true)           
                .Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {           
            services.AddControllers();
            //Pøidání gateway engine: Ocelot
            services.AddOcelot(_config);
            services.AddSwaggerForOcelot(_config);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerForOcelotUI(_config, opt =>
            {
                opt.DownstreamSwaggerHeaders = new[]
                {
                        new KeyValuePair<string, string>("Key", "Value"),
                        new KeyValuePair<string, string>("Key2", "Value2"),
                    };
            }).UseOcelot().Wait();

            app.UseRouting();
            app.UseAuthorization();
        }
        
    }
}
