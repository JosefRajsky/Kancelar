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

        public async Task<IActionResult> Index(int? rok, int? mesic)
        {
            if (rok == null) {
                rok = DateTime.Today.Year;
            }
            if (mesic != null) {
                ViewBag.Mesic = mesic;
            }
            var model = await client.GetListAsync();            
           
           
            foreach (var item in model.Where(w => w.Rok == rok))
            {
                var uzivatel = await UzivatelServis.GetAsync(item.UzivatelId);
                if (uzivatel != null) {
                    item.UzivatelCeleJmeno = $"{uzivatel.Prijmeni} {uzivatel.Jmeno}";
                }
                
            }

            return View(model.Where(w => w.Rok == rok).ToList());
        }

        public async Task<IActionResult> Mesic(int? mesic, int? rok)
        {
            if (rok == 0)
            {
                rok = DateTime.Today.Year;
            }
            if (mesic != null)
            {
                ViewBag.Mesic = mesic;
            }
            var kalendarList = await client.GetListAsync();
            var model = kalendarList.Where(k => k.Rok == rok);

            foreach (var item in model)
            {
                var uzivatel = await UzivatelServis.GetAsync(item.UzivatelId);
                item.UzivatelCeleJmeno = $"{uzivatel.Prijmeni} {uzivatel.Jmeno}";
            }

            return View(model);
        }






    }
}