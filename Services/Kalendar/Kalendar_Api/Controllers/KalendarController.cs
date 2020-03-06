using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using EventLibrary;
using Kalendar_Api.Models;
using Kalendar_Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Kalendar_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class KalendarController : ControllerBase
    {
        private readonly IKalendarRepository _repository;
        public KalendarController(IKalendarRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<ActionResult<Kalendar>> Get(int id)
        {
            var response = await _repository.Get(id.ToString());     
            return Ok(response);
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<IEnumerable<KalendarModel>> GetList()
        {
            var response = await _repository.GetList(); 
            return response;
        }

        [HttpPut]
        [Route("Add")]
        public async Task Add(CommandKalendarCreate cmd)      
        {         
         await _repository.Add(cmd);            
        }

       
        [HttpPost]
        [Route("Update")]
        public async Task Update(CommandKalendarUpdate cmd)
        {
            await _repository.Update(cmd);        
        }
    }
}
