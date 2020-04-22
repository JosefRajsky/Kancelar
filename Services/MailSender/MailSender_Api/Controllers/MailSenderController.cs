using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandHandler;
using MailSender_Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MailSender_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class MailSenderlateController : ControllerBase
    {
        private readonly IRepository _repository;
       
        public MailSenderlateController(IRepository repository)
        {
            _repository = repository;
          
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<Mail> Get(Guid id)
        {
            var response = await _repository.Get(id);            
            return response;
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<List<Mail>> GetList() {

            var response = await _repository.GetList(); 
            return response;
        }

        [HttpPost]
        [Route("Add")]
        public async Task Add(CommandMailSenderCreate cmd)      
        { 
         await _repository.Add(cmd);            
        }

        [HttpDelete]
        [Route("Remove")]
        public async Task Delete(CommandMailSenderRemove cmd)
        {
           await _repository.Remove(cmd);   
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(CommandMailSenderUpdate cmd)
        {
            await _repository.Update(cmd);        
        }
    }
}
