using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Udalost_Api.Entities;
using Udalost_Api.Models;
using Udalost_Api.Repositories;

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
        public async Task<ActionResult<UdalostModel>> Get(Guid id) => Ok(await _udalostRepository.Get(id));

        [HttpGet]
        [Route("GetList")]
        public async Task<List<UdalostModel>> GetList() {
            var model =await _udalostRepository.GetList();

            //TODO: Dodělat derivát z entity Model a vyčítat z něj.
            var response = new List<UdalostModel>();
            foreach (var item in model)
            {
                var u = new UdalostModel();
                u.Id = item.Id ;
                u.UdalostTypId = item.UdalostTypId;
                u.Popis = item.Popis ;
                u.Nazev = item.Nazev;
                u.UzivatelCeleJmeno = item.UzivatelCeleJmeno;
                u.DatumOd = item.DatumOd ;
                u.DatumDo = item.DatumDo ;
                u.UzivatelId = item.UzivatelId ;

                response.Add(u);
            }
            return response;
        }

        [HttpPut]
        [Route("Add")]
        public async Task Add(CommandUdalostCreate cmd)
        {
            await _udalostRepository.Add(cmd);
        }

        [HttpPost]
        [Route("Remove")]
        public async Task Remove(CommandUdalostRemove cmd)
        {
            await _udalostRepository.Remove(cmd);
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(CommandUdalostUpdate cmd)
        {
            await _udalostRepository.Update(cmd);
        }
    }
}
