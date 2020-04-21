using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using CommandHandler;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Import.Repositories;

namespace Import.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class ImportController : ControllerBase
    {
        private readonly IRepository _repository;
        
        public ImportController(IRepository repository)
        {
            _repository = repository;
          
        }
        [HttpGet]
        [Route("Get")]
        public ActionResult Get()
        {
            return Ok(_repository.ImportUzivatel(string.Empty));
        }
        [HttpGet]
        [Route("Import")]
        public ActionResult DoImport()
        {
            return Ok( _repository.ImportUzivatel(string.Empty));
        }    

        [HttpPost]
        [Route("ImportUzivatel")]
        public async Task ImportUzivatel(string uzivatele)      
        { 
         await _repository.ImportUzivatel(uzivatele);            
        }

       
    }
}
