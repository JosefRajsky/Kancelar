using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cinnost_Api.Repositories;

namespace Cinnost_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class CinnostlateController : ControllerBase
    {
        private readonly IRepository _repository;
       
        public CinnostlateController(IRepository repository)
        {
            _repository = repository;
          
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<Cinnost> Get(Guid id)
        {
            var response = await _repository.Get(id);            
            return response;
        }
        [HttpGet]
        [Route("GetList")]
        public async Task<List<Cinnost>> GetList() {

            var response = await _repository.GetList(); 
            return response;
        }
        [HttpPost]
        [Route("Add")]
        public async Task Add(CommandCinnostCreate cmd)     
        { 
         await _repository.Add(cmd);            
        }

        [HttpDelete]
        [Route("Remove")]
        public async Task Delete(CommandCinnostRemove cmd)
        {
           await _repository.Remove(cmd);   
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(CommandCinnostUpdate cmd)
        {
            await _repository.Update(cmd);        
        }
    }
}
