using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KancelarWeb.Interfaces;
using KancelarWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace KancelarWeb.Controllers
{
    public class DochazkaController : Controller
    {
        IDochazkaProvider provider;
        public DochazkaController(IDochazkaProvider baseProvider)
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
               
        public async Task<IActionResult> AddPrichod(string prichod)
        {
            Random rnd = new Random();
            var model = new DochazkaModel();
            model.UzivatelId = rnd.Next(100, 1000);
            model.Datum = (Convert.ToBoolean(prichod)) ? DateTime.Now.AddHours(-rnd.Next(1, 5)) : DateTime.Now.AddHours(rnd.Next(1, 4));
            model.Prichod = Convert.ToBoolean(prichod);
            model.UzivatelCeleJmeno = "Jmeno Prijmeni";

            await provider.Add(model);
            return RedirectToAction("Index");
        }
        public IActionResult Remove(string id)
        {
            provider.Delete(Convert.ToInt32(id));
            return RedirectToAction("Index");
        }
    }
}