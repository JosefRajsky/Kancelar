using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vykaz_Api.Repositories;

namespace Vykaz_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class VykazController : ControllerBase
    {
        private readonly IRepository _repository;
       
        public VykazController(IRepository repository)
        {
            _repository = repository;
          
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<Vykaz> Get(Guid id)
        {
            var response = await _repository.Get(id);            
            return response;
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<List<Vykaz>> GetList() {

            var response = await _repository.GetList(); 
            return response;
        }

        [HttpPost]
        [Route("Add")]
        public async Task Add(CommandVykazCreate cmd)      
        { 
         await _repository.Add(cmd);            
        }

        [HttpDelete]
        [Route("Remove")]
        public async Task Delete(CommandVykazRemove cmd)
        {
           await _repository.Remove(cmd);   
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(CommandVykazUpdate cmd)
        {
            await _repository.Update(cmd);        
        }
    }
}
