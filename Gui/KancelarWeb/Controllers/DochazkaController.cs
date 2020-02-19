using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KancelarWeb.Interfaces;
using KancelarWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KancelarWeb.Controllers
{
    public class DochazkaController : Controller
    {
        IApiProvider provider;
        string apibase;
        
        public DochazkaController(IApiProvider apiProvider)
        {
            provider = apiProvider;
            apibase = "ApiDochazka";
        }
        public async Task<IActionResult> Index()
        {
            var response = await provider.List<string>(apibase);
            var model = new List<DochazkaViewModel>();
            if (response == null){
                return View(model);
            }            
            var result = JsonConvert.DeserializeObject<List<DochazkaViewModel>>(response);
            model.AddRange(result);
            return View(model);          
        }
        public async Task<IActionResult> Detail(int id)
        {
            var response = await provider.Get<string>(id,apibase);
            var model = new DochazkaViewModel();
            if (response == null)
            {
                return View(model);
            }
            var result = JsonConvert.DeserializeObject<DochazkaViewModel>(response.ToString());
            model = result;
            return View(model);
        }

        public async Task<IActionResult> AddPrichod(string prichod)
        {            
            Random rnd = new Random();
            var model = new DochazkaViewModel();
            model.UzivatelId = rnd.Next(100, 1000);
            model.Datum = (Convert.ToBoolean(prichod)) ? DateTime.Now.AddHours(-rnd.Next(1, 5)) : DateTime.Now.AddHours(rnd.Next(1, 4));
            model.Prichod = Convert.ToBoolean(prichod);
            model.UzivatelCeleJmeno = "Jmeno Prijmeni";
            await provider.Add(model, apibase);            
            return RedirectToAction("Index"); 
        }
        public async Task<IActionResult> Remove(string id)
        {
            await provider.Remove(Convert.ToInt32(id), apibase);
                return RedirectToAction("Index");
        }
    }
}