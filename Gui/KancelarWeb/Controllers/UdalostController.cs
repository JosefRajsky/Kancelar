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
using Newtonsoft.Json.Linq;

namespace KancelarWeb.Controllers
{
    public class UdalostController : Controller
    {
        IApiProvider provider;
        string defaultBaseUri;

        public UdalostController(IApiProvider apiProvider)
        {
            provider = apiProvider;
            defaultBaseUri = "http://udalostapi/udalost/";
        }

        public async Task<IActionResult> Index()
        {

            var response = await provider.List<string>(defaultBaseUri);
            var model = new List<UdalostModel>();
            if (response == null)
            {
                return View(model);
            }
            var result = JsonConvert.DeserializeObject<List<UdalostModel>>(response.ToString());
            model.AddRange(result);
            return View(model);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var response = await provider.Get<string>(id, defaultBaseUri);
            var model = new DochazkaModel();
            if (response == null)
            {
                return View(model);
            }
            var result = JsonConvert.DeserializeObject<DochazkaModel>(response.ToString());
            model = result;
            return View(model);
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
            provider.Add(model, defaultBaseUri);

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Update(UdalostModel model)
        {
            provider.Update(model, defaultBaseUri);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Remove(string id)
        {
            await provider.Remove(Convert.ToInt32(id), defaultBaseUri);
            return RedirectToAction("Index");
        }
    }
}