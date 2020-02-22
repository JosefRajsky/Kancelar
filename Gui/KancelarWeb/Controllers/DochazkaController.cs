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
      
        string apibase;
        
        public DochazkaController()
        {
            apibase = "http://webapiocelot/Api/Dochazka/";
        }

        public async Task<IActionResult> Index()
        {

            var model = new List<DochazkaViewModel>();
            var client = new HttpClient();
            var host = string.Format("{0}", apibase);
            client.BaseAddress = new Uri(host);
            var response = await client.GetAsync("GetList");

            //TODO: Jak zjistí jak vypadá Model, které api poskytuje? ... zatím se shoduje se ServiceModel
            model = JsonConvert.DeserializeObject<List<DochazkaViewModel>>(await response.Content.ReadAsStringAsync());
            if (model == null)
            {
                return View(new List<DochazkaViewModel>());
            }
            return View(model);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var model = new DochazkaViewModel();

            var client = new HttpClient();
            var host = string.Format("{0}", apibase);
            client.BaseAddress = new Uri(host);
            var response = await client.GetAsync("Get");
            model = JsonConvert.DeserializeObject<DochazkaViewModel>(await response.Content.ReadAsStringAsync());
           
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
          
            var client = new HttpClient();
            var host = string.Format("{0}", apibase);
            client.BaseAddress = new Uri(host);
            await client.PostAsJsonAsync("Add", model);
              
            return RedirectToAction("Index"); 
        }
       
        public async Task<IActionResult> Remove(int id)
        {
          
            var client = new HttpClient();
            var host = string.Format("{0}", apibase);
            client.BaseAddress = new Uri(host);
            await client.DeleteAsync(string.Format("Remove/{0}", id));

            return RedirectToAction("Index");
        }
    }
}