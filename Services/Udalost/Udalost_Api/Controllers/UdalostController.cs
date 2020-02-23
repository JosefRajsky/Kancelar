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
        [Route("Get/{id?}")]
        public async Task<ActionResult<UdalostModel>> Get(int id) => Ok(await _udalostRepository.Get(id));
        [HttpGet]
        [Route("GetList")]
        public async Task<ActionResult<UdalostModel>> GetList() {
            var model =await _udalostRepository.GetList();

            //TODO: Dodělat derivát z entity Model a vyčítat z něj.
            var response = new List<UdalostModel>();
            foreach (var item in model)
            {
                var u = new UdalostModel();
                u.Id = item.Id ;
                u.UdalostTypId = item.UdalostTypid ;
                u.Popis = item.Popis ;
                u.DatumOd = item.DatumOd ;
                u.DatumDo = item.DatumDo ;
                u.UzivatelId = item.UzivatelId ;
                u.UzivatelCeleJmeno = " --- " ;

                response.Add(u);
            }
            return Ok(response);
        }
        [HttpPost]
        [Route("Add")]
        public async Task Add(UdalostModel model)
        {
            await _udalostRepository.Add(model);
        }

        [HttpDelete]
        [Route("Remove/{id?}")]
        public async Task Remove(int id)
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
