using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Opravneni_Api.Repositories;

namespace Opravneni_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class OpravnenilateController : ControllerBase
    {
        private readonly IRepository _repository;
       
        public OpravnenilateController(IRepository repository)
        {
            _repository = repository;
          
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<ActionResult<Pravo>> Get(Guid id)
        {
            var response = await _repository.Get(id);            
            return Ok(response);
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<List<Pravo>> GetList() {

            var response = await _repository.GetList(); 
            return response;
        }

        [HttpPost]
        [Route("Add")]
        public async Task Add(CommandOpravneniCreate cmd)      
        { 
         await _repository.Add(cmd);            
        }

        [HttpDelete]
        [Route("Remove")]
        public async Task Delete(CommandOpravneniRemove cmd)
        {
           await _repository.Remove(cmd);   
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(CommandOpravneniUpdate cmd)
        {
            await _repository.Update(cmd);        
        }
    }
}
