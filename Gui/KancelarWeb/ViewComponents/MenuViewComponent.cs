using KancelarWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
       
        public MenuViewComponent()
        {
         
        }

        public IViewComponentResult Invoke()
        {
           
            return View();
        }
    }


}
