using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using KancelarWeb.Services;
using KancelarWeb.ViewModels;
using Microsoft.AspNetCore.Blazor;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KancelarWeb.Controllers
{
    public class SoucastController : Controller
    {
        readonly SoucastClient client;
        public SoucastController()
        {
            client = new SoucastClient();
        }

        public async Task<IActionResult> Index()
        {
            var model = await client.GetListAsync();
            return View(model);
        }
        public async Task<IActionResult> Detail(Guid id)
        {
            var model = await client.GetAsync(id);
            return View(model);
        }
        public async Task<IActionResult> Edit(Guid? id)
        {
            var model = new Soucast();
            if (id != null)
            {
                model = await client.GetAsync(new Guid(id.ToString()));
            }              
            return View(model);        
        }

        public async Task<IActionResult> Add([FromForm]Soucast model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit");
            }
            if (model.SoucastId != Guid.Empty) {
                var command = new CommandSoucastUpdate()
                {
                     Nazev = model.Nazev,
                     ParentId = model.ParentId,
                     SoucastId = model.SoucastId,
                     Zkratka = model.Zkratka       
                };
                await client.UpdateAsync(command);
            } else { 
            var command = new CommandSoucastCreate()
            {
                Nazev = model.Nazev,
                ParentId = model.ParentId,
                SoucastId = model.SoucastId,
                Zkratka = model.Zkratka
            };
            await client.AddAsync(command);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update([FromForm]Soucast model)
        {
            var command = new CommandSoucastUpdate()
            {
                Nazev = model.Nazev,
                ParentId = model.ParentId,
                SoucastId = model.SoucastId,
                Zkratka = model.Zkratka
            };
            await client.UpdateAsync(command);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(Guid id)
        {
            await client.DeleteAsync(new CommandSoucastRemove() {
                SoucastId = id 
            });

            return RedirectToAction("Index");
        }
       
      

    }
}