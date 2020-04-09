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
    public class UzivatelController : Controller
    {
        UzivatelClient client;
        public UzivatelController()
        {
            client = new UzivatelClient();
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
            var model = new Uzivatel();
            model.DatumNarozeni = DateTime.Today;
            if (id != null) {
                model = await client.GetAsync(new Guid(id.ToString()));
            }          
          
            ViewBag.PohlaviList = new SelectList(Enum.GetValues(typeof(EPohlavi)).Cast<EPohlavi>().Select(v => new SelectListItem
            {
                Text = v.Description(),
                Value = v.Description()
            }).ToList(), "Value", "Text");
                                  
            return View(model);
        }

        public async Task<IActionResult> Add([FromForm]Uzivatel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit");
            }
            if (model.UzivatelId != Guid.Empty) {
                var command = new CommandUzivatelUpdate()
                {

                    DatumNarozeni = model.DatumNarozeni,
                    Email = model.Email,
                    Jmeno = model.Jmeno,
                    Pohlavi = model.Pohlavi,
                    Prijmeni = model.Prijmeni,
                    Telefon = model.Telefon,
                    TitulPred = model.TitulPred,
                    TitulZa = model.TitulZa,
                    UzivatelId = model.UzivatelId
                };
                await client.UpdateAsync(command);
            } else { 
            var command = new CommandUzivatelCreate()
            {
                DatumNarozeni = (model.DatumNarozeni == null)? DateTime.MinValue: model.DatumNarozeni,
                Email = model.Email,
                Jmeno = model.Jmeno,
                Pohlavi = model.Pohlavi,
                Prijmeni = model.Prijmeni,
                Telefon = model.Telefon,
                TitulPred = model.TitulPred,
                TitulZa = model.TitulZa,
                UzivatelId = model.UzivatelId
            };
            await client.AddAsync(command);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update([FromForm]Uzivatel model)
        {
            var command = new CommandUzivatelUpdate()
            {
                DatumNarozeni = (model.DatumNarozeni == null) ? DateTime.MinValue : model.DatumNarozeni,
                Email = model.Email,
                Jmeno = model.Jmeno,
                Pohlavi = model.Pohlavi,
                Prijmeni = model.Prijmeni,
                Telefon = model.Telefon,
                TitulPred = model.TitulPred,
                TitulZa = model.TitulZa,
                UzivatelId = model.Id
            };
            await client.UpdateAsync(command);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(Guid id)
        {
            await client.DeleteAsync(new CommandUzivatelRemove() {
                UzivatelId = id 
            });

            return RedirectToAction("Index");
        }
       
      

    }
}