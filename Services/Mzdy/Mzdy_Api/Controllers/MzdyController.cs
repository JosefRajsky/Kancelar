using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mzdy_Api.Repositories;

namespace Mzdy_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class MzdylateController : ControllerBase
    {
        private readonly IRepository _repository;
       
        public MzdylateController(IRepository repository)
        {
            _repository = repository;
          
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<Mzda> Get(Guid id)
        {
            var response = await _repository.Get(id);            
            return response;
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<List<Mzda>> GetList() {

            var response = await _repository.GetList(); 
            return response;
        }

        [HttpPost]
        [Route("Add")]
        public async Task Add(CommandMzdyCreate cmd)      
        { 
         await _repository.Add(cmd);            
        }

        [HttpDelete]
        [Route("Remove")]
        public async Task Delete(CommandMzdyRemove cmd)
        {
           await _repository.Remove(cmd);   
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(CommandMzdyUpdate cmd)
        {
            await _repository.Update(cmd);        
        }
    }
}
