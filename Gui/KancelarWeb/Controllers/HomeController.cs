using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KancelarWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View("Index");
        }
        public IActionResult Error()
        {
            ViewBag.ErrorMsg = "Na stránce došlo k chybě";
            return View("Index");
        }
    }
}