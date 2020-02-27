using KancelarWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.ViewComponents
{
    public class StromViewComponent : ViewComponent
    {
        private readonly UzivatelClient client;
        public StromViewComponent()
        {
            client = new UzivatelClient();
        }               

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = await client.GetListAsync();
          
            return View(model);
        }
    }

   
}
