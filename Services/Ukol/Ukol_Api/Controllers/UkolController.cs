using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ukol_Api.Repositories;

namespace Ukol_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class UkollateController : ControllerBase
    {
        private readonly IRepository _repository;
       
        public UkollateController(IRepository repository)
        {
            _repository = repository;
          
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<ActionResult<Ukol>> Get(Guid id)
        {
            var response = await _repository.Get(id);            
            return Ok(response);
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<List<Ukol>> GetList() {

            var response = await _repository.GetList(); 
            return response;
        }

        [HttpPost]
        [Route("Add")]
        public async Task Add(CommandUkolCreate cmd)      
        { 
         await _repository.Add(cmd);            
        }

        [HttpDelete]
        [Route("Remove")]
        public async Task Delete(CommandUkolRemove cmd)
        {
           await _repository.Remove(cmd);   
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(CommandUkolUpdate cmd)
        {
            await _repository.Update(cmd);        
        }
    }
}
