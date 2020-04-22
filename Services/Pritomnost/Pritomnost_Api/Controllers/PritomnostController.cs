using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Pritomnost_Api.Models;
using Pritomnost_Api.Repositories;

namespace Pritomnost_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class PritomnostController : ControllerBase
    {
        private readonly IRepository _repository;
        public PritomnostController(IRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<Pritomnost> Get(Guid id)
        {
            var response = await _repository.Get(id);     
            return response;
        }

        [HttpGet]
        [Route("GetList")]
        public async Task<List<Pritomnost>> GetList()
        {
             
            return await _repository.GetList();
        }

   
    }
}
