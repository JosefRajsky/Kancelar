
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
                    this.ConfirmCreated(envelope);
                    break;
                //case MessageType.UzivatelCreate:                       
                //        //-------------Description: Deserializace zprávy do správného typu a odeslání k uložení do DB; 
                //        this.Add(JsonConvert.DeserializeObject<CommandUzivatelCreate>(envelope.Event));                   
                //    break;
                //case MessageType.UzivatelRemove:                   
                //        this.Remove(JsonConvert.DeserializeObject<CommandUzivatelRemove>(envelope.Event));                  
                //    break;
                //case MessageType.UzivatelUpdate:                  
                //        this.Update(JsonConvert.DeserializeObject<CommandUzivatelUpdate>(envelope.Event));                 
                //    break;
            }
        }
        public void ConfirmCreated(Message msg) {   
         _repository.Add(JsonConvert.DeserializeObject<CommandUzivatelCreate>(msg.Command), msg.Guid);
        }
   
        public void Add(CommandUzivatelCreate cmd, Guid replayed)
        {
            _repository.Add(cmd, replayed);
        }

        public void Remove(CommandUzivatelRemove cmd, Guid replayed)
        {
            _repository.Remove(cmd,replayed);


        }
        public void Update(CommandUzivatelUpdate cmd, Guid replayed)
        {
            _repository.Update(cmd, replayed);
        }
        //public void AcceptCommand(Guid guid) {
        //    _repository.AcceptCommand(guid);
        //}
    }
}
