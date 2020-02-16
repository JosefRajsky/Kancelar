using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KancelarWeb.Interfaces;
using KancelarWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace KancelarWeb.Controllers
{
    public class DochazkaController : Controller
    {
        IDochazkaProvider provider;
        public DochazkaController(IDochazkaProvider baseProvider)
        {
            provider = baseProvider;
        }
        public IActionResult Index()
        {
            var result = provider.GetList();
            if (!result.Any())
            {
                ViewBag.error = "Seznam je prázdný";
            }

            return View(result);
          
        }
               
        public IActionResult AddPrichod(string prichod)
        {

            #region TEST CODE
            for (int i = 1; i < 5; i++)
            {
                Random testrnd= new Random();

                var test = new DochazkaModel();
                test.UzivatelId = testrnd.Next(100, 1000);
                test.Datum = (Convert.ToBoolean(prichod)) ? DateTime.Now.AddHours(-testrnd.Next(1, 5)) : DateTime.Now.AddHours(testrnd.Next(1, 4));
              test.Prichod = Convert.ToBoolean(prichod);
               test.UzivatelCeleJmeno = "Jmeno Prijmeni";

                provider.Add(test);
            }
            #endregion

            Random rnd = new Random();
            var model = new DochazkaModel();
            model.UzivatelId = rnd.Next(100, 1000);
            model.Datum = (Convert.ToBoolean(prichod)) ? DateTime.Now.AddHours(-rnd.Next(1, 5)) : DateTime.Now.AddHours(rnd.Next(1, 4));
            model.Prichod = Convert.ToBoolean(prichod);
            model.UzivatelCeleJmeno = "Jmeno Prijmeni";
            provider.Add(model);




            return RedirectToAction("Index");
        }
        public IActionResult Remove(string id)
        {
            provider.Delete(Convert.ToInt32(id));
            return RedirectToAction("Index");
        }
    }
}