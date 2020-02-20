using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using KancelarWeb.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using UdalostLibrary;
using System.Net.Http;

namespace KancelarWeb.Controllers
{
    public class UdalostController : Controller
    {
       
        string apibase;

        public UdalostController()
        {
           
            apibase = "http://webapi/ApiUdalost/";
        }

        public async Task<IActionResult> Index()
        {
            var model = new List<UdalostViewModel>();

            var client = new HttpClient();
            var host = string.Format("{0}", apibase);
            client.BaseAddress = new Uri(host);
            var response = await client.GetAsync("GetList");

            //TODO: Jak zjistí jak vypadá Model, které api poskytuje? ... zatím se shoduje se ServiceModel
            model = JsonConvert.DeserializeObject<List<UdalostViewModel>>(await response.Content.ReadAsStringAsync());
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var model = new UdalostViewModel();

            var client = new HttpClient();
            var host = string.Format("{0}", apibase);
            client.BaseAddress = new Uri(host);
            var response = await client.GetAsync("Get");
            model = JsonConvert.DeserializeObject<UdalostViewModel>(await response.Content.ReadAsStringAsync());

            if (response == null)
            {
                return View(model);
            }           
            return View(model);
        }

        public async Task<IActionResult> Add(UdalostViewModel model)
        {
            var client = new HttpClient();
            var host = string.Format("{0}", apibase);
            client.BaseAddress = new Uri(host);
            await client.PostAsJsonAsync("Add", model);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Update(UdalostViewModel model)
        {
            var client = new HttpClient();
            var host = string.Format("{0}", apibase);
            client.BaseAddress = new Uri(host);
            await client.PutAsJsonAsync("Update", model);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Remove(string id)
        {
            var client = new HttpClient();
            var host = string.Format("{0}", apibase);
            client.BaseAddress = new Uri(host);
            await client.DeleteAsync(string.Format("Remove/{0}", id));
            return RedirectToAction("Index");
        }
        public IActionResult Edit()
        {
            var model = new UdalostViewModel();
            model.UdalostTypList = new List<SelectListItem>();
            foreach (var item in (UdalostTyp[])Enum.GetValues(typeof(UdalostTyp)))
            {

                model.UdalostTypList.Add(new SelectListItem() { Text = EmumExtension.GetDescription(item), Value = item.ToString() });
            }
            return View(model);
        }

   
    }
}