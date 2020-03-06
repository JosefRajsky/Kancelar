using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using EventLibrary;
using EventStore_Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EventStore_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class EventStoreController : ControllerBase
    {
        private readonly IEventStoreRepository _repository;
        public EventStoreController(IEventStoreRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<ActionResult<Message>> Get(int id)
        {
             
            return await _repository.Get(id.ToString());
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<List<Message>> GetList()
        {
    
            return await _repository.GetList();
        }

 

        
    }
}
