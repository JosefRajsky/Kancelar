using CommandHandler;
using Dochazka_Api.Models;
using Dochazka_Api.Repositories;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dochazka_Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class DochazkaController : ControllerBase
    {
        private readonly IRepository _repository;

        public DochazkaController(IRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        [Route("Get/{id?}")]
        public async Task<Dochazka> Get(Guid id)
        {
            var response = await _repository.Get(id);
            return response;
        }
        [HttpGet]
        [Route("GetList")]
        public async Task<List<Dochazka>> GetList()
        {
            var response = await _repository.GetList();
            return response;
        }

        [HttpPost]
        [Route("Add")]
        public async Task Add(CommandDochazkaCreate cmd)
        {
            await _repository.Add(cmd);
        }

        [HttpDelete]
        [Route("Remove")]
        public async Task Delete(CommandDochazkaRemove cmd)
        {
            await _repository.Remove(cmd);
        }
        [HttpPost]
        [Route("Update")]
        public async Task Update(CommandDochazkaUpdate cmd)
        {
            await _repository.Update(cmd);
        }
    }
}
