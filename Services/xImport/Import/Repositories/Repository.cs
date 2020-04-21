
using CommandHandler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Import.Repositories
{
    public class Repository : IRepository
    {
      
        private MessageHandler _handler;
        public Repository(Publisher publisher)
        {                   
            _handler = new MessageHandler(publisher);
        }
   
        public async Task ImportUzivatel(string uzivatele)
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








