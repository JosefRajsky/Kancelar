using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Udalost_Api.Entities;
using Udalost_Api.Repositories;

namespace Udalost_Api.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class UdalostController : ControllerBase
    {
        private readonly IRepository _repository;
        public UdalostController(IRepository udalostService)
        {
            _repository = udalostService;
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<Udalost> Get(Guid udalostId) =>await _repository.Get(udalostId);

        [HttpGet]
        [Route("GetList")]
        public async Task<List<Udalost>> GetList() {
           return await _repository.GetList();
        }
        [HttpPut]
        [Route("Add")]
        public async Task Add(CommandUdalostCreate cmd)
        {
            await _repository.Add(cmd);
        }

        [HttpPost]
        [Route("Remove")]
        public async Task Remove(CommandUdalostRemove cmd)
        {
            await _repository.Remove(cmd);
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(CommandUdalostUpdate cmd)
        {
            await _repository.Update(cmd);
        }
    }
}
