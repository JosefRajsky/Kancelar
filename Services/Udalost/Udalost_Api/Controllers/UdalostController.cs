using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        [Route("Get")]
        public ActionResult<Udalost> Get(int id)
        {
            var result = _udalostRepository.Get(id);
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }
        [HttpGet]
        [Route("GetList")]
        public ActionResult<List<Udalost>> GetList() {
            var result = _udalostRepository.GetList().ToList();
            if (result == null || !result.Any()){
                return NotFound();
            }
            return result;
        }

        [HttpPut]
        [Route("Add")]
        public async Task Add(UdalostModel model)
        {
            await _udalostRepository.Add(model);           
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task Delete(int id)
        {
           await _udalostRepository.Delete(Convert.ToInt32(id));   
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(UdalostModel model)
        {
            await _udalostRepository.Update(model);   
        }
    }
}
