using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KancelarWeb.Services;
using KancelarWeb.ViewModels;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KancelarWeb.Controllers
{
    public class DochazkaController : Controller
    {
        DochazkaClient client;
        public DochazkaController() 
        {
            client = new DochazkaClient();
        }

        public async Task<IActionResult> Index()
        {          
            var model = await client.GetListAsync();
            return View(model);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var model = await client.GetAsync(id);
            return View(model);
        }
        
      
        public async Task<IActionResult> AddPrichod(string prichod)
        {            
            Random rnd = new Random();
            var model = new DochazkaModel();
            model.UzivatelId = rnd.Next(100, 1000);
            model.Datum = (Convert.ToBoolean(prichod)) ? DateTime.Now.AddHours(-rnd.Next(1, 5)) : DateTime.Now.AddHours(rnd.Next(1, 4));
            model.Prichod = Convert.ToBoolean(prichod);
            model.UzivatelCeleJmeno = "Jmeno Prijmeni";
            await client.AddAsync(model);
              
            return RedirectToAction("Index"); 
        }
       
        public async Task<IActionResult> Remove(int id)
        {
            await client.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}