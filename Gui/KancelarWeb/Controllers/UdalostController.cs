using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;



using System.Text;
using Newtonsoft.Json;
using KancelarWeb.Interfaces;
using KancelarWeb.Models;
using KancelarWeb.Services;

namespace KancelarWeb.Controllers
{
    public class UdalostController : Controller
    {
        IUdalostProvider provider;
        public UdalostController(IUdalostProvider baseProvider)
        {
            provider = baseProvider;
        }

        public IActionResult Index()
        {
            var result = provider.GetList();
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
        public IActionResult Add(UdalostModel model)
        {
            provider.Add(model);
            
            return RedirectToAction("Index");
        }
        public IActionResult Remove(string id)
        {
            provider.Remove(Convert.ToInt32(id));
            return RedirectToAction("Index");
        }
    }
}