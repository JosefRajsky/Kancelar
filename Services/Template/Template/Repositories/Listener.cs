
using CommandHandler;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Template.Repositories
{
    public class Listener 
    {        
        private readonly IRepository _repository;
        public Listener(IRepository repository)
        {
            _repository = repository;
            CheckOnStartUp();
        }
        public async void CheckOnStartUp()
        {
            await _repository.RequestEvents(Guid.Empty);
        }
        public void AddCommand(string message)
        {
            //-------------Description: Deserializace Json objektu na základní typ zprávy
            var envelope = JsonConvert.DeserializeObject<Message>(message);
            //-------------Description: Rozhodnutí o typu získazné zprávy. Typ vázaný na Enum z knihovny
            switch (envelope.MessageType)
            {
                case MessageType.HealingStreamProvided:
                    //-------------Description: Deserializace zprávy do správného typu a odeslání k uložení do DB; 
                    var ev = JsonConvert.DeserializeObject<HealingStreamProvided>(envelope.Event);
                    _repository.ReplayEvents(ev.MessageList, envelope.EntityId);
                    break;
                case MessageType.UzivatelCreated:

                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventTempCreated>(envelope.Event).EventId, envelope.EntityId);
                    break;
                case MessageType.UzivatelUpdated:
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventTempCreated>(envelope.Event).EventId, envelope.EntityId);
                    break;      
            }
        }



    }
}
