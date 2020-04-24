
using CommandHandler;


using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Uzivatel_Api.Repositories
{
    public class Listener 
    {        
        private readonly IRepository _repository;
        //Description: Vytvoření posluchače a přidání repositáře
        public Listener(IRepository repository)
        {
            _repository = repository;
            //Description: Spuštění metody kontroly stavu
            CheckOnStartUp();
        }
        //Description: Kontrola stavu entit v DB
        public async void CheckOnStartUp()
        {
            //Description: Publikace o nekonzistentní stavu a vyžádání obnovy
            await _repository.RequestEvents(Guid.Empty);
        }
        //Description: Metoda zpracování konzumovaných zpráv
        public void AddCommand(string message)
        {
            //Description: Deserializace Json objektu na základní typ zprávy
            var envelope = JsonConvert.DeserializeObject<Message>(message);
            //Description: Rozhodnutí o typu získazné zprávy. Typ vázaný na Enum z knihovny
            switch (envelope.MessageType)
            {
                //Description: Indikace typu obnovovacího proudu zpráv
                case MessageType.HealingStreamProvided:
                    //Description: Deserializace zprávy do správného typu a odeslání k uložení do DB
                    var ev = JsonConvert.DeserializeObject<HealingStreamProvided>(envelope.Event);
                    //Description: Předání obnovovacího proudu pro zpracování
                    _repository.ReplayEvents(ev.MessageList, envelope.EntityId);
                    break;
                    //Description: Reakce na vytvoření uživatele, předání ke kontrole stavu
                case MessageType.UzivatelCreated:
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventUzivatelCreated>(envelope.Event).EventId, envelope.EntityId);
                    break;
                //Description: Reakce na upravení uživatele uživatele, předání ke kontrole stavu
                case MessageType.UzivatelUpdated:
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventUzivatelUpdated>(envelope.Event).EventId, envelope.EntityId);
                    break;      
            }
        }
    }
}
