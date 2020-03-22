using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandHandler;
using EventLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nastaveni_Api.Entities;
using Nastaveni_Api.Repositories;

namespace Nastaveni_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class NastaveniController : ControllerBase
    {
        private readonly INastaveniRepository _repository;
       
        public NastaveniController(INastaveniRepository repository)
        {
            _repository = repository;
          
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<ActionResult<Nastaveni>> Get(int id)
        {
            var response = await _repository.Get(id.ToString());           
            return Ok(response);
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<List<Nastaveni>> GetList() {

            var model = await _repository.GetList();

   
            return model;
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
