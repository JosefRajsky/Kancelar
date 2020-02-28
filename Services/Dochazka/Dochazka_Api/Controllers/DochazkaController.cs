using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dochazka_Api.Models;
using Dochazka_Api.Repositories;
using EventLibrary;
using Microsoft.AspNetCore.Mvc;

namespace Dochazka_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class DochazkaController : ControllerBase
    {
        private readonly IDochazkaRepository _dochazkaRepository;
        public DochazkaController(IDochazkaRepository dochazkaService)
        {
            _dochazkaRepository = dochazkaService;
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<ActionResult<DochazkaModel>> Get(int id)
        {
            var item = await _dochazkaRepository.Get(id.ToString());
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

            var model = await _dochazkaRepository.GetList();

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
         await _dochazkaRepository.Add(cmd);            
        }

        [HttpPost]
        [Route("Remove")]
        public async Task Delete(CommandDochazkaRemove cmd)
        {
           await _dochazkaRepository.Remove(cmd);   
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(CommandDochazkaUpdate cmd)
        {
            await _dochazkaRepository.Update(cmd);        
        }
    }
}
