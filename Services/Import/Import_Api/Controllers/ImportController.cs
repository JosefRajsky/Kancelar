using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandHandler;
using Import_Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Import_Api.Controllers
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
        [HttpPut]
        [Route("Add")]
        public async Task Add()
        {
            await _repository.Add(string.Empty);
        }

    }
}
