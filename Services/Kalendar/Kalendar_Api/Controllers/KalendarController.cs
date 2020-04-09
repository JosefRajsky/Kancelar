using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandHandler;
using Kalendar_Api.Models;
using Kalendar_Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Kalendar_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class KalendarController : ControllerBase
    {
        private readonly IRepository _repository;
        public KalendarController(IRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<ActionResult<Kalendar>> Get(Guid id)
        {
            var response = await _repository.Get(id);     
            return Ok(response);
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<List<Kalendar>> GetList()
        {
            var response = await _repository.GetList(); 
            return response;
        }

        //[HttpPut]
        //[Route("Add")]
        //public async Task Add(CommandKalendarCreate cmd)      
        //{         
        // await _repository.Add(cmd,true);            
        //}

       
        //[HttpPost]
        //[Route("Update")]
        //public async Task Update(CommandKalendarUpdate cmd)
        //{
        //    await _repository.Update(cmd, true);        
        //}
    }
}
