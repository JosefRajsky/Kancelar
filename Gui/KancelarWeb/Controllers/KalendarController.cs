using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KancelarWeb.CommandsModels;
using KancelarWeb.Services;
using KancelarWeb.ViewModels;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KancelarWeb.Controllers
{
    public class KalendarController : Controller
    {
        KalendarClient client;
        UzivatelClient UzivatelServis;
        public KalendarController()
        {
            client = new KalendarClient();
            UzivatelServis = new UzivatelClient();
        }

        public async Task<IActionResult> Index()
        {
            var model = await client.GetListAsync();

            foreach (var item in model)
            {
                var uzivatel = await UzivatelServis.GetAsync(item.UzivatelId);
                item.CeleJmeno = $"{uzivatel.Prijmeni} {uzivatel.Jmeno}";
            }
            return View(model);
        }
       
     

       
      

    }
}