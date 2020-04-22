using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandHandler;
using Transfer_Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Transfer_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImportExportController : ControllerBase
    {
        private readonly IRepository _repository;
        public ImportExportController(IRepository repository)
        {
            _repository = repository;
        }
        [HttpGet]
        [Route("Get")]
        public string Get()
        {
            _repository.Add(string.Empty);
            return string.Empty;
        }
        [HttpPut]
        [Route("Add")]
        public void Add(string json)
        {
            _repository.Add(string.Empty);
        }

    }
}
