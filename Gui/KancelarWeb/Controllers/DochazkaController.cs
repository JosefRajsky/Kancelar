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
        private ResiliencyHelper _resiliencyHelper;
        public DochazkaController(IDochazkaProvider baseProvider)
        {
            provider = baseProvider;
            _resiliencyHelper = new ResiliencyHelper(provider);
        }
        public async Task<IActionResult> Index()
        {
            var result = await provider.GetList();
            if (!result.Any())
            {
                ViewBag.error = "Seznam je prázdný";
            }

            return View(result);
          
        }
               
        public async Task<IActionResult> AddPrichod(string prichod)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            
        }
            Random rnd = new Random();
            var model = new DochazkaModel();
            model.UzivatelId = rnd.Next(100, 1000);
            model.Datum = (Convert.ToBoolean(prichod)) ? DateTime.Now.AddHours(-rnd.Next(1, 5)) : DateTime.Now.AddHours(rnd.Next(1, 4));
            model.Prichod = Convert.ToBoolean(prichod);
            model.UzivatelCeleJmeno = "Jmeno Prijmeni";

            var result = await provider.Add(model);
            if (result) {               
                return RedirectToAction("Index");
            } 
            else {
                return RedirectToAction("Error","Home");
            }
            
        }
        public async Task<IActionResult> Remove(string id)
        {

            return await _resiliencyHelper.ExecuteResilient(async () =>
            {
                var response = await provider.Delete(Convert.ToInt32(id));
                return RedirectToAction("Index");
            }, RedirectToAction("Error", "Home"));

           
        }
    }
}