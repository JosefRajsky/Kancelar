using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Soucast_Api.Repositories;

namespace Soucast_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class SoucastlateController : ControllerBase
    {
        private readonly IRepository _repository;
       
        public SoucastlateController(IRepository repository)
        {
            _repository = repository;
          
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<Soucast> Get(Guid id)
        {
            var response = await _repository.Get(id);            
            return response;
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<List<Soucast>> GetList() {

            var response = await _repository.GetList(); 
            return response;
        }

        [HttpPost]
        [Route("Add")]
        public async Task Add(CommandSoucastCreate cmd)      
        { 
         await _repository.Add(cmd);            
        }

        [HttpDelete]
        [Route("Remove")]
        public async Task Delete(CommandSoucastRemove cmd)
        {
           await _repository.Remove(cmd);   
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(CommandSoucastUpdate cmd)
        {
            await _repository.Update(cmd);        
        }
    }
}
