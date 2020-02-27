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
        public async Task<IActionResult> Detail(int id)
        {
            var model = await client.GetAsync(id);
            return View(model);
        }


        public async Task<IActionResult> Add([FromForm]UzivatelModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Edit");
            }
            var command = new CommandUzivatelCreate()
            {
                DatumNarozeni = (model.DatumNarozeni == null)? DateTime.MinValue: model.DatumNarozeni,
                Email = model.Email,
                Foto = model.Foto,
                Jmeno = model.Jmeno,
                Pohlavi = model.Pohlavi,
                Prijmeni = model.Prijmeni,
                Telefon = model.Telefon,
                TitulPred = model.TitulPred,
                TitulZa = model.TitulZa,
                UzivatelId = model.Id
            };
            await client.AddAsync(command);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(UzivatelModel model)
        {
            var command = new CommandUzivatelUpdate()
            {
                DatumNarozeni =  (model.DatumNarozeni == null)? DateTime.MinValue : model.DatumNarozeni,
                Email = model.Email,
                Foto = model.Foto,
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

        public async Task<IActionResult> Remove(int id)
        {
            await client.DeleteAsync(new CommandUzivatelRemove() {
                UzivatelId = id 
            });
            return RedirectToAction("Index");
        }
       
        public IActionResult Edit()
        {
            var model = new UzivatelModel();
            model.DatumNarozeni = DateTime.Today;
            
            return View(model);
        }
    }
}