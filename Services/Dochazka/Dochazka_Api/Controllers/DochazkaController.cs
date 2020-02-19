using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dochazka_Api.Entities;
using Dochazka_Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            var result = _dochazkaRepository.Get(id.ToString());    
            return result;
        }
        [HttpGet]
        [Route("GetList")]
        public string GetList() {
            var result = _dochazkaRepository.GetList().ToList();

            //TODO: Dočasný obšuk. Dodělat derivát z entity Model a vyčítat z něj.
            var listDochazka = new List<DochazkaModel>();
            foreach (var item in result)
            {
                var d = new DochazkaModel(item);
                listDochazka.Add(d);
            }
           


            return JsonConvert.SerializeObject(listDochazka);
        }

        [HttpPost]
        [Route("Add")]
        public void Add(string model)      
        {
         
         _dochazkaRepository.Add(model);            
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
