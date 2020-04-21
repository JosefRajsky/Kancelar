
using CommandHandler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Import_Api.Repositories
{
    public class Repository : IRepository
    {
     
        private MessageHandler _handler;
        public Repository(Publisher publisher)
        {         
            _handler = new MessageHandler(publisher);
        }

        public async Task Add(string uzivatele)
        {
            var ev = new EventUzivatelCreated()
            {
                EventId = Guid.NewGuid(),
                UzivatelId = Guid.NewGuid(),                
                EventCreated = DateTime.Now,
                ImportedId = string.Empty,
                TitulPred = "tit",
                Jmeno = "Jmeno",
                Prijmeni ="Prijmeni",
                TitulZa = "tit za",
                Pohlavi = "muž",
                DatumNarozeni = DateTime.Now,
                Email = "rajsky@centrum.cz",
                Telefon = "123456",
                Generation = 0,
            };              
               
                await _handler.PublishEvent(ev, MessageType.UzivatelCreated, ev.EventId, null, ev.Generation, ev.UzivatelId);
            
        }



    }

}








