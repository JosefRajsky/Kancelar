using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Api.Entities;
using Web_Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Web_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class ApiDochazkaController : ControllerBase
    {
        private readonly IApiDochazkaRepository repository;
        public ApiDochazkaController(IApiDochazkaRepository dochazkaService)
        {
            repository = dochazkaService;
        }
        [HttpGet]
        [Route("Get")]
        public ActionResult<Dochazka> Get(int id)
        {
            var result = repository.Get(id.ToString());
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }
        [HttpGet]
        [Route("GetList")]
        public string GetList() {
            var result = repository.GetList().ToList();
            if (result == null || !result.Any()){
                return string.Empty;
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
            return JsonConvert.SerializeObject(dochazkaList);
        }

        [HttpPut]
        [Route("Add")]
        public async Task Add(DochazkaModel model)      
        {
        await repository.Add(model);            
        }

        [HttpDelete]
        [Route("Remove")]
        public async Task Delete(string id)
        {
           await repository.Remove(id);   
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(DochazkaModel model)
        {
            await repository.Update(model);        
        }
    }
}
