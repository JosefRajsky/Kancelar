using Dochazka_Service.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Configuration.Json;
using System;
using Udalost_Service;
using System.IO;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Dochazka_Service.Handlers;
using Microsoft.Extensions.Logging;

namespace Dochazka_Service
{
    class Program
    {
        public static IConfiguration Configuration { get; }
        public Program(IConfiguration configuration)
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            
        }
        public void Configure(IApplicationBuilder app)
        {

        }
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<DochazkaDbContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:DbConn"]));
            //services.AddTransient<IDochazkaRepository, DochazkaRepository>();
            //services.AddSingleton<EventBroker>();
        }
        static void Main(string[] args)
        {
            try
            {
                var serviceProvider = new ServiceCollection()              
               .AddSingleton<IEventBroker, EventBroker>()
               .BuildServiceProvider();
                serviceProvider.GetService<IEventBroker>().Start();               
            }
            catch (Exception e)
            {

                throw e;
            }
        }   
    }  
}

