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
            await client.AddAsync(model);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(UdalostModel model)
        {
            await client.UpdateAsync(model);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Remove(int id)
        {
            await client.DeleteAsync(id);
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