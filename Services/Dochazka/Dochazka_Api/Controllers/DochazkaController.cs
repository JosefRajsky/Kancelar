using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dochazka_Api.Entities;
using Dochazka_Api.Repositories;
using DochazkaLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

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
        [Route("Get")]
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
        public async Task<ActionResult<DochazkaModel>> GetList() {

            var model =await _dochazkaRepository.GetList();

        //TODO: Dočasný obšuk. Dodělat derivát z entity Model a vyčítat z něj.
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
            return Ok(response);
        }

        [HttpPost]
        [Route("Add")]
        public async Task Add(DochazkaModel model)      
        {         
         await _dochazkaRepository.Add(model);            
        }

        [HttpDelete]
        [Route("Remove")]
        public async Task Delete(string id)
        {
           await _dochazkaRepository.Remove(id);   
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(DochazkaModel model)
        {
            await _dochazkaRepository.Update(model);        
        }
    }
}
