
using CommandHandler;

using EventLibrary;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Uzivatel_Api.Repositories
{
    public class Listener : IListener
    {
        
        private readonly IUzivatelRepository _repository;
        public Listener(IUzivatelRepository repository)
        {
            _repository = repository;          
         
      
        
        }
        public void AddCommand(string message)
        {
            //-------------Description: Deserializace Json objektu na základní typ zprávy
            var envelope = JsonConvert.DeserializeObject<Message>(message);
            //-------------Description: Rozhodnutí o typu získazné zprávy. Typ vázaný na Enum z knihovny
            switch (envelope.MessageType)
            {
                case MessageType.UzivatelCreated:
                    //-------------Description: Deserializace zprávy do správného typu a odeslání k uložení do DB; 
                    this.Restore(envelope);
                    break;
                case MessageType.UzivatelUpdated:
                    //-------------Description: Deserializace zprávy do správného typu a odeslání k uložení do DB; 
                    this.ReUpdate(envelope);
                    break;
                case MessageType.UzivatelRemoved:
                    //-------------Description: Deserializace zprávy do správného typu a odeslání k uložení do DB; 
                    this.Remove(envelope);
                    break;       
            }
        }
        public void Restore(Message msg) {   
         _repository.Restore(JsonConvert.DeserializeObject<CommandUzivatelCreate>(msg.Command), msg.EntityId);
        }
        public void Remove(Message msg)
        {
            _repository.Remove(JsonConvert.DeserializeObject<CommandUzivatelRemove>(msg.Command), msg.Guid);
        }
        public void ReUpdate(Message msg)
        {
            _repository.ReUpdate(JsonConvert.DeserializeObject<CommandUzivatelUpdate>(msg.Command), msg.EntityId);
        }

      
    }
}
