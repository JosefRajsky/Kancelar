using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using KancelarWeb.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using UdalostLibrary;
using System.Net.Http;
using KancelarWeb.Services;

namespace KancelarWeb.Controllers
{
    public class UdalostController : Controller
    {

        UdalostClient client;
        public UdalostController()
        {
            client = new UdalostClient();
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
        public async Task<IActionResult> Add( UdalostModel model)
        {
            var command = new CommandUdalostCreate() {
                UzivatelId = model.UzivatelId,
                DatumDo = model.DatumDo,
                DatumOd = model.DatumDo,
                DatumZadal = DateTime.Today,
                Popis = model.Popis,
                UdalostTypId = model.Id
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
        public IActionResult Edit()
        {
            var model = new UdalostModel();
            model.DatumDo = DateTime.Today;
            model.DatumOd = DateTime.Today;
            model.UdalostTypList = new SelectList(Enum.GetValues(typeof(UdalostTyp)));
            //model.UdalostTypList = new List<SelectListItem>();
            //foreach (var item in (UdalostTyp[])Enum.GetValues(typeof(UdalostTyp)))
            //{
            //    model.UdalostTypList.Add(new SelectListItem() { Text = EmumExtension.GetDescription(item), Value = (int)item });
            //}
            return View(model);
        }

   
    }
}