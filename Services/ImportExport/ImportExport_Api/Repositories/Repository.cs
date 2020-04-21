
using CommandHandler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;


namespace ImportExport_Api.Repositories
{
    public class Repository : IRepository
    {
     
        private MessageHandler _handler;
        public Repository(Publisher publisher)
        {         
            _handler = new MessageHandler(publisher);
        }
        public async Task LastEventCheck(Guid eventId, Guid entityId)
        {
            await Task.Run(() => {
              
            });

        }
        public async Task RequestEvents(Guid? entityId)
        {
            var msgTypes = new List<MessageType>();

            if (msgTypes.Any()) {
                await _handler.RequestReplay("ImportExport.ex", entityId, msgTypes);
            }
          
        }
        public async Task ReplayEvents(List<string> stream, Guid? entityId)
        {
           await Task.Run(()=>{
               var messages = new List<Message>();
               foreach (var item in stream)
               {
                   messages.Add(JsonConvert.DeserializeObject<Message>(item));
               }
               var replayOrderedStream = messages.OrderBy(d => d.Created);
               foreach (var msg in replayOrderedStream)
               {
                   switch (msg.MessageType)
                   {

                   }
               }
           }); 
        
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








