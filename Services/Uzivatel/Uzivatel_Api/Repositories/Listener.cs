
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
        
        private readonly IUzivatelRepository _repository;
        public Listener(IUzivatelRepository repository)
        {
            _repository = repository;
            CheckOnStartUp();
        }
        public async void CheckOnStartUp()
        {
            await _repository.RequestReplay(Guid.Empty);
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
                    this.Replay(envelope);
                    break;
                case MessageType.UzivatelCreated:
                   
                    this.CCreate(envelope);
                    break;
                case MessageType.UzivatelUpdated:
           
                    this.CUpdate(envelope);
                    break;
      
            }
        }
        public void CCreate(Message msg) {   
         _repository.ConfirmAdd(JsonConvert.DeserializeObject<EventUzivatelCreated>(msg.Event), msg.EntityId);
        }
        public void CUpdate(Message msg)
        {
            _repository.ConfirmUpdate(JsonConvert.DeserializeObject<EventUzivatelUpdated>(msg.Event), msg.EntityId);
        }
        public void Replay(Message msg)
        {
           var ev = JsonConvert.DeserializeObject<HealingStreamProvided>(msg.Event);    
            _repository.ReplayStream(ev.MessageList, msg.EntityId);
        }


    }
}
