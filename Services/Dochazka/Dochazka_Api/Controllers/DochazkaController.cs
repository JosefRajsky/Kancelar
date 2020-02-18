using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dochazka_Api.Entities;
using Dochazka_Api.Repositories;
using Microsoft.AspNetCore.Http;
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
        [Route("Get")]
        public ActionResult<Dochazka> Get(int id)
        {
            var result = _dochazkaRepository.Get(id);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }
        [HttpGet]
        [Route("GetList")]
        public ActionResult<List<DochazkaModel>> List() {
            var result = _dochazkaRepository.GetList().ToList();
            if (result == null || !result.Any()){
                return NotFound();
            }

            //TODO: WORKAROUND
            var dochazkaList = new List<DochazkaModel>();
            foreach (var item in result)
            {
                var dochazka = new DochazkaModel()
                {
                    Id = item.Id,
                    Prichod = item.Prichod,
                    Datum = new DateTime(item.Tick),
                    UzivatelId = item.UzivatelId,
                    UzivatelCeleJmeno = "test"
                };
                dochazkaList.Add(dochazka);
            }
            return dochazkaList;
        }

        [HttpPut]
        [Route("Add")]
        public async Task Add(DochazkaModel model)      
        {
        await _dochazkaRepository.Add(model);            
        }

        [HttpDelete]
        [Route("Remove")]
        public async Task Delete(int id)
        {
           await _dochazkaRepository.Remove(Convert.ToInt32(id));   
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(DochazkaModel model)
        {
            await _dochazkaRepository.Update(model);        
        }
    }
}
