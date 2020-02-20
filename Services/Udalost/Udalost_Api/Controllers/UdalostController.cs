using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Udalost_Api.Entities;
using Udalost_Api.Repositories;
using UdalostLibrary.Models;

namespace Udalost_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class UdalostController : ControllerBase
    {
        private readonly IUdalostRepository _udalostRepository;
        public UdalostController(IUdalostRepository udalostService)
        {
            _udalostRepository = udalostService;
        }
        [HttpGet]
        [Route("Get")]
        public ActionResult<Udalost> Get(int id)
        {
            var result = _udalostRepository.Get(id);
            return result;
        }
        [HttpGet]
        [Route("GetList")]
        public string GetList() {
            var result = _udalostRepository.GetList().ToList();

            //TODO: Dočasný obšuk. Dodělat derivát z entity Model a vyčítat z něj.
            var listDochazka = new List<UdalostModel>();
            foreach (var item in result)
            {
                var u = new UdalostModel();
                u.Id = item.Id ;
                u.UdalostTypId = item.UdalostTypid ;
                u.Popis = item.Popis ;
                u.DatumOd = item.DatumOd ;
                u.DatumDo = item.DatumDo ;
                u.UzivatelId = item.UzivatelId ;
                u.UzivatelCeleJmeno = " --- " ;

                listDochazka.Add(u);
            }
            return JsonConvert.SerializeObject(listDochazka);
        }
        [HttpPost]
        [Route("Add")]
        public async Task Add(UdalostModel model)
        {
            await _udalostRepository.Add(model);
        }

        [HttpDelete]
        [Route("Remove")]
        public async Task Remove(string id)
        {
            await _udalostRepository.Remove(id);
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(UdalostModel model)
        {
            await _udalostRepository.Update(model);
        }
    }
}
