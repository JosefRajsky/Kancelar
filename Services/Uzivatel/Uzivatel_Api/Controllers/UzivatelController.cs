using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using CommandHandler;
using EventLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uzivatel_Api.Repositories;

namespace Uzivatel_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class UzivatelController : ControllerBase
    {
        private readonly IUzivatelRepository _repository;
       
        public UzivatelController(IUzivatelRepository repository)
        {
            _repository = repository;
          
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<ActionResult<UzivatelModel>> Get(int id)
        {
            var item = await _repository.Get(id.ToString());
            var response = new UzivatelModel(item);      
            return Ok(response);
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<List<UzivatelModel>> GetList() {

            var model = await _repository.GetList();

            //TODO: Vytvořit Wiew (derivát) a nečíst přímo z entity.
            var response = new List<UzivatelModel>();
            foreach (var item in model)
            {
                var d = new UzivatelModel(item);
                
                response.Add(d);
            }    
            return response;
        }

        [HttpPost]
        [Route("Add")]
        public async Task Add(CommandUzivatelCreate cmd)      
        { 
         await _repository.Add(cmd);            
        }

        [HttpDelete]
        [Route("Remove")]
        public async Task Delete(CommandUzivatelRemove cmd)
        {
           await _repository.Remove(cmd);   
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(CommandUzivatelUpdate cmd)
        {
            await _repository.Update(cmd);        
        }
    }
}
