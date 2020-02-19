using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web_Api.Entities;
using Web_Api.Models;
using Web_Api.Repositories;

namespace Web_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class ApiUdalostController : ControllerBase
    {
        private readonly IApiUdalostRepository _repository;
        public ApiUdalostController(IApiUdalostRepository udalostService)
        {
            _repository = udalostService;
        }
        [HttpGet]
        [Route("Get")]
        public ActionResult<Udalost> Get(int id)
        {
            var result = _repository.Get(id);          
            return result;
        }
        [HttpGet]
        [Route("GetList")]
        public ActionResult<List<Udalost>> GetList() {
            var result = _repository.GetList().ToList();
            
            return result;
        }

        [HttpPut]
        [Route("Add")]
        public async Task Add(UdalostModel model)
        {
            await _repository.Add(model);           
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task Delete(int id)
        {
           await _repository.Delete(Convert.ToInt32(id));   
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(UdalostModel model)
        {
            await _repository.Update(model);   
        }
    }
}
