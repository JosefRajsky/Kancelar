using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using KancelarWeb.Services;
using System.Linq;
using System.Security.Cryptography.Xml;
using KancelarWeb.ViewModels;

namespace KancelarWeb.Controllers
{
    public class AktivitaController : Controller
    {

        AktivitaClient client;
        UzivatelClient uzivatelService;
        public AktivitaController()
        {
            client = new AktivitaClient();
            uzivatelService = new UzivatelClient();
        }
        public async Task<IActionResult> Index()
        {
            var model = await client.GetListAsync();           
            return View(model);
        }
        public async Task<IActionResult> Detail(Guid aktivitaId, Guid id)
        {
            var model = await client.GetAsync(aktivitaId,id.ToString());
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Aktivita model)
        {
            var command = new CommandAktivitaCreate() {
                UzivatelId = model.UzivatelId,  
                Nazev = model.Nazev,
                UzivatelCeleJmeno = model.UzivatelCeleJmeno,
                DatumOd = model.DatumOd,
                DatumDo = model.DatumDo,
                DatumZadal = DateTime.Today,
                Popis = model.Popis,
                AktivitaTypId = model.AktivitaTypId
            };
            await client.AddAsync(command);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(Aktivita model)
        {
            var command = new CommandAktivitaUpdate()
            {
                AktivitaId = model.Id,
                UzivatelId = model.UzivatelId,
                DatumDo = model.DatumDo,
                Nazev = model.Nazev,
                UzivatelCeleJmeno = model.UzivatelCeleJmeno,
                DatumOd = model.DatumDo,
                DatumZadal = DateTime.Today,
                Popis = model.Popis,
                AktivitaTypId = model.AktivitaTypId
            };
            await client.UpdateAsync(command);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Remove(Guid id)
        {
            var command = new CommandAktivitaRemove() { AktivitaId = id };
            await client.RemoveAsync(command);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> EditAsync()
        {
            var model = new Aktivita();
            model.DatumDo = DateTime.Today;
            model.DatumOd = DateTime.Today;

            ViewBag.AktivitaTypList = new SelectList(Enum.GetValues(typeof(EAktivitaTyp)).Cast<EAktivitaTyp>().Select(v => new SelectListItem
            {
                Text = v.Description(),
                Value = v.ToString()
            }).ToList(), "Value", "Text");

            ViewBag.UzivatelList = new List<SelectListItem>();
            foreach (var item in await uzivatelService.GetListAsync())
            {
                ViewBag.UzivatelList.Add(new SelectListItem { Value = item.Id.ToString(), Text = $"{item.Prijmeni} {item.Jmeno}" });
            }           
            return View(model);
        }

   
    }
}