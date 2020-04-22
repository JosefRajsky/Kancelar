using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nastaveni_Api.Repositories;

namespace Nastaveni_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class NastavenilateController : ControllerBase
    {
        private readonly IRepository _repository;
       
        public NastavenilateController(IRepository repository)
        {
            _repository = repository;
          
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<ActionResult<Pravidlo>> Get(Guid id)
        {
            var response = await _repository.Get(id);            
            return Ok(response);
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<List<Pravidlo>> GetList() {

            var response = await _repository.GetList(); 
            return response;
        }

        [HttpPost]
        [Route("Add")]
        public async Task Add(CommandNastaveniCreate cmd)      
        { 
         await _repository.Add(cmd);            
        }

        [HttpDelete]
        [Route("Remove")]
        public async Task Delete(CommandNastaveniRemove cmd)
        {
           await _repository.Remove(cmd);   
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(CommandNastaveniUpdate cmd)
        {
            await _repository.Update(cmd);        
        }
    }
}
