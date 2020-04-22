using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aktivita_Api.Repositories;
using CommandHandler;

namespace Aktivita_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class AktivitaController : ControllerBase
    {
        private readonly IRepository _repository;
       
        public AktivitaController(IRepository repository)
        {
            _repository = repository;
          
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<Aktivita> Get(Guid id)
        {
            var response = await _repository.Get(id);            
            return response;
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<List<Aktivita>> GetList() {

            var response = await _repository.GetList(); 
            return response;
        }

        [HttpPost]
        [Route("Add")]
        public async Task Add(CommandAktivitaCreate cmd)      
        { 
         await _repository.Add(cmd);            
        }

        [HttpDelete]
        [Route("Remove")]
        public async Task Delete(CommandAktivitaRemove cmd)
        {
           await _repository.Remove(cmd);   
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(CommandAktivitaUpdate cmd)
        {
            await _repository.Update(cmd);        
        }
    }
}
