using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;



using System.Text;
using Newtonsoft.Json;
using KancelarWeb.Interfaces;
using KancelarWeb.Models;

namespace KancelarWeb.Controllers
{
    public class HomeController : Controller
    {
        IKancelarWebProvider provider;
        public HomeController(IKancelarWebProvider kancelarWebProvider)
        {
            provider = kancelarWebProvider;
        }
        public IActionResult Index()
        {
            var result = provider.UdalostList();
            if (!result.Any())
            {
                ViewBag.error = "Seznam je prázdný";
            }

            return View(result);
        }
        public IActionResult Edit()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddUdalost(Udalost udalost)
        {
            udalost.Datum = DateTime.Today;
            var result = provider.UdalostAdd(udalost);
            if (!result)
            {
                ViewBag.error = "Seznam je prázdný";
            }
            return RedirectToAction("Index");
        }
        public IActionResult RemoveUdalost(string id)
        {          
            return RedirectToAction("Index");
        }
    }
}