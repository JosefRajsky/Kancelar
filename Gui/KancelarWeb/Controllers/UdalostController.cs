﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using KancelarWeb.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http;
using KancelarWeb.Services;
using KancelarWeb.CommandsModels;
using System.Linq;

namespace KancelarWeb.Controllers
{
    public class UdalostController : Controller
    {

        UdalostClient client;
        UzivatelClient clientUzivatel;
        public UdalostController()
        {
            client = new UdalostClient();
            clientUzivatel = new UzivatelClient();
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
        [HttpPost]
        public async Task<IActionResult> Add([FromForm]UdalostModel model)
        {
            var command = new CommandUdalostCreate() {
                UzivatelId = model.UzivatelId,                 
                DatumDo = model.DatumDo,
                Nazev = model.Nazev,
                UzivatelCeleJmeno = model.UzivatelCeleJmeno,
                DatumOd = model.DatumDo,
                DatumZadal = DateTime.Today,
                Popis = model.Popis,
                UdalostTypId = model.UdalostTypId
            };
            await client.AddAsync(command);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(UdalostModel model)
        {
            var command = new CommandUdalostUpdate()
            {
                UdalostId = model.Id,
                UzivatelId = model.UzivatelId,
                DatumDo = model.DatumDo,
                Nazev = model.Nazev,
                UzivatelCeleJmeno = model.UzivatelCeleJmeno,
                DatumOd = model.DatumDo,
                DatumZadal = DateTime.Today,
                Popis = model.Popis,
                UdalostTypId = model.Id
            };
            await client.UpdateAsync(command);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Remove(int id)
        {
            var command = new CommandUdalostRemove() { UdalostId = id };
            await client.RemoveAsync(command);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> EditAsync()
        {
            var model = new UdalostModel();
            model.DatumDo = DateTime.Today;
            model.DatumOd = DateTime.Today;

            ViewBag.UdalostTypList = new SelectList(Enum.GetValues(typeof(EUdalostTyp)).Cast<EUdalostTyp>().Select(v => new SelectListItem
            {
                Text = v.Description(),
                Value = v.ToString()
            }).ToList(), "Value", "Text");

            ViewBag.UzivatelList = new List<SelectListItem>();
            foreach (var item in await clientUzivatel.GetListAsync())
            {
                ViewBag.UzivatelList.Add(new SelectListItem { Value = item.Id.ToString(), Text = $"{item.Prijmeni} {item.Jmeno}" });
            }           
            return View(model);
        }

   
    }
}