using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using System.Text;
using Newtonsoft.Json;
using KancelarWeb.Interfaces;
using KancelarWeb.Models;
using KancelarWeb.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using UdalostLibrary;

namespace KancelarWeb.Controllers
{
    public class UdalostController : Controller
    {
        IUdalostProvider provider;
        public UdalostController(IUdalostProvider baseProvider)
        {
            provider = baseProvider;
        }

        public async Task<IActionResult> Index()
        {
            var result = await provider.GetList();
            if (!result.Any())
            {
                ViewBag.error = "Seznam je prázdný";
            }

            return View(result);
        }
        public IActionResult Edit()
        {
            var model = new UdalostModel();
            model.UdalostTypList = new List<SelectListItem>();
            foreach (var item in (UdalostTyp[])Enum.GetValues(typeof(UdalostTyp)))
            {
             
                model.UdalostTypList.Add(new SelectListItem() { Text = EmumExtension.GetDescription(item), Value = item.ToString() });
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Add(UdalostModel model)
        {
            provider.Add(model);
            
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Update(UdalostModel model)
        {
            provider.Update(model);

            return RedirectToAction("Index");
        }
        public IActionResult Remove(string id)
        {
            provider.Remove(Convert.ToInt32(id));
            return RedirectToAction("Index");
        }
    }
}