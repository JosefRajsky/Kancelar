using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Transfer_Api.Repositories;

namespace Transfer_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly IRepository _repository;
       
        public TransferController(IRepository repository)
        {
            _repository = repository;
          
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<Transfer> Get(Guid id)
        {
            var response = await _repository.Get(id);            
            return response;
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<List<Transfer>> GetList() {

            var response = await _repository.GetList(); 
            return response;
        }

        [HttpPost]
        [Route("Add")]
        public async Task Add(CommandTransferCreate cmd)      
        { 
         await _repository.Add(cmd);            
        }

        [HttpDelete]
        [Route("Remove")]
        public async Task Delete(CommandTransferRemove cmd)
        {
           await _repository.Remove(cmd);   
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(CommandTransferUpdate cmd)
        {
            await _repository.Update(cmd);        
        }
    }
}
