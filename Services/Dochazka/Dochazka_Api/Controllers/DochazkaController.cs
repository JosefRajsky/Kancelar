using Dochazka_Api.Models;
using Dochazka_Api.Repositories;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dochazka_Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class DochazkaController : ControllerBase
    {
        private readonly IDochazkaRepository _repository;
        public DochazkaController(IDochazkaRepository dochazkaService)
        {
            _repository = dochazkaService;
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<ActionResult<DochazkaModel>> Get(Guid id)
        {
            var item = await _repository.Get(id);
            var response = new DochazkaModel() {
                Id = item.Id,
                Datum = new DateTime(item.Rok, item.Mesic, item.Den),
                CteckaId = string.Empty,
                UzivatelId = item.UzivatelId,
                UzivatelCeleJmeno = " - ",
                Prichod = item.Prichod,
            };
             
      
            return Ok(response);
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<List<DochazkaModel>> GetList()
        {

            var model = await _repository.GetList();

            //TODO: Vytvořit Wiew a nečíst přímo z entity.
            var response = new List<DochazkaModel>();
            foreach (var item in model)
            {
                var d = new DochazkaModel();
                d.Id = item.Id;
                d.Datum = new DateTime(item.Rok, item.Mesic, item.Den);
                d.CteckaId = string.Empty;
                d.UzivatelId = item.UzivatelId;
                d.UzivatelCeleJmeno = " - ";
                d.Prichod = item.Prichod;
                response.Add(d);
            }
            return response;
        }

        [HttpPut]
        [Route("Add")]
        public async Task Add(CommandDochazkaCreate cmd)      
        {         
         await _repository.Add(cmd, true);            
        }

        [HttpPost]
        [Route("Remove")]
        public async Task Delete(CommandDochazkaRemove cmd)
        {
           await _repository.Remove(cmd, true);   
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(CommandDochazkaUpdate cmd)
        {
            await _repository.Update(cmd, true);        
        }
    }
}
