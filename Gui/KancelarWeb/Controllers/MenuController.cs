using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KancelarWeb.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Index()
        {
            return PartialView("Index");
        }
    }
}