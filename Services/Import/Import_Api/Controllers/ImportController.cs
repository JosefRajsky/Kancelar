using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandHandler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Import_Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImportController : ControllerBase
    {
        private MessageHandler _handler;
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

     

        public ImportController(Publisher publisher)
        {
            _handler = new MessageHandler(publisher);
        }

        [HttpGet]
        public void Get()
        {
            ImportUzivatel(string.Empty);
        }
        [HttpPost]
         public async void ImportUzivatel(string uzivatele)
        {
            var ev = new EventUzivatelCreated()
            {
                EventId = Guid.NewGuid(),
                UzivatelId = Guid.NewGuid(),
                EventCreated = DateTime.Now,
                ImportedId = "595",
                TitulPred = "bc",
                Jmeno = "Josef",
                Prijmeni = "Rajsky",
                TitulZa = string.Empty,
                Pohlavi = "Muž",
                DatumNarozeni = DateTime.Now,
                Email = "rajsky@gmail.com",
                Telefon = "+420737327222",
                Generation = 0,
            };
            await _handler.PublishEvent(ev, MessageType.UzivatelCreated, ev.EventId, null, ev.Generation, ev.UzivatelId);
            
        }
    }
}
