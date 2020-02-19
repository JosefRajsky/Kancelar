using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KancelarWeb.Interfaces;
using KancelarWeb.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KancelarWeb
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore();
            services.AddRazorPages();
            services.AddScoped<IUdalostProvider, UdalostProvider>();            
            services.AddScoped<IDochazkaProvider, DochazkaProvider>();            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
     
            app.UseStaticFiles();  
            app.UseRouting();
            app.UseStatusCodePages();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                                  name: "default",
                                  pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
