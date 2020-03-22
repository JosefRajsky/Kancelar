using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KancelarWeb.CommandsModels;
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
        UzivatelClient UzivatelServis;
        public DochazkaController() 
        {
            client = new DochazkaClient();
            UzivatelServis = new UzivatelClient();
        }

        public async Task<IActionResult> Index()
        {          
            var model = await client.GetListAsync();

            //TODO: pridat do DB, nevolat addhoc
            foreach (var item in model)
            {
                var uzivatel = await UzivatelServis.GetAsync(item.UzivatelId);
                if (uzivatel != null)
                {
                    item.UzivatelCeleJmeno = $"{uzivatel.Prijmeni} {uzivatel.Jmeno}";
                }

            }


            return View(model);
        }
        public async Task<IActionResult> Detail(Guid id)
        {
            var model = await client.GetAsync(id);
            var uzivatel = await UzivatelServis.GetAsync(model.UzivatelId);
            model.UzivatelCeleJmeno = $"{uzivatel.Prijmeni} {uzivatel.Jmeno}";
            return View(model);
        }
        
      
        public async Task<IActionResult> AddPrichod(string prichod, Guid uzivatelId)
        {            
            Random rnd = new Random();
            var model = new CommandDochazkaCreate()
            {
                CteckaId = "",
                Datum = DateTimeOffset.Now,
                Prichod = Convert.ToBoolean(prichod),
                UzivatelId = uzivatelId,
            };
           
            await client.AddAsync(model);
              
            return RedirectToAction("Index"); 
        }
       
        public async Task<IActionResult> Remove(Guid id)
        {
            var model = new CommandDochazkaRemove()
            {
                DochazkaId = id
            };
           
            await client.DeleteAsync(model);

            return RedirectToAction("Index");
        }
    }
}