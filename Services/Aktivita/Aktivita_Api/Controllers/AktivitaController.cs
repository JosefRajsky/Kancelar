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
        public AktivitaController(IRepository AktivitaService)
        {
            _repository = AktivitaService;
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<Aktivita> Get(Guid AktivitaId) => await _repository.Get(AktivitaId);

        [HttpGet]
        [Route("GetList")]
        public async Task<List<Aktivita>> GetList()
        {
            return await _repository.GetList();
        }
        [HttpPut]
        [Route("Add")]
        public async Task Add(CommandAktivitaCreate cmd)
        {
            await _repository.Add(cmd);
        }

        [HttpPost]
        [Route("Remove")]
        public async Task Remove(CommandAktivitaRemove cmd)
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
