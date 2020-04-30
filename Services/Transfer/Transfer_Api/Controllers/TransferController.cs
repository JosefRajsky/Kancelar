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
      
        [HttpPost]
        [Route("ImportUzivatel")]
        public async Task Add(List<CommandUzivatelCreate> cmds)      
        { 
         await _repository.ImportUzivatel(cmds);            
        }
        [HttpPost]
        [Route("ImportSoucast")]
        public async Task Add(List<CommandSoucastCreate> cmds)
        {
            await _repository.ImportSoucast(cmds);
            
           
        }
      
    }
}
