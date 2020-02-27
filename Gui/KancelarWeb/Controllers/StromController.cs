using System;

using System.Threading.Tasks;
using KancelarWeb.Services;
using Microsoft.AspNetCore.Mvc;



namespace KancelarWeb.Controllers
{
    public class StromController : Controller
    {
        UzivatelClient client;
        public StromController()
        {
            client = new UzivatelClient();
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> List()
        {
            var model = await client.GetListAsync();
            return ViewComponent("_Strom", model);

        }
    }
}