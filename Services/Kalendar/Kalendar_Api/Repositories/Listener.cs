


using CommandHandler;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Kalendar_Api.Repositories
{
    public class Listener 
    {
        //string _BaseUrl;
        private readonly IRepository _repository;
        
        public Listener(IRepository repository)
        {
            _repository = repository;
           
        }

        public void AddCommand(string message)
        {
            var envelope = JsonConvert.DeserializeObject<Message>(message);
            switch (envelope.MessageType)
            {
                case MessageType.HealingStreamProvided:
                    //-------------Description: Deserializace zprávy do správného typu a odeslání k uložení do DB; 
                    var ev = JsonConvert.DeserializeObject<HealingStreamProvided>(envelope.Event);
                    _repository.ReplayEvents(ev.MessageList, envelope.EntityId);
                    break;
                case MessageType.UzivatelCreated:
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventUzivatelCreated>(envelope.Event), envelope.EntityId,);
                    break;
                case MessageType.UzivatelUpdated:
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventUzivatelUpdated>(envelope.Event), envelope.EntityId);
                    break;
                case MessageType.UzivatelRemoved:
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventUzivatelDeleted>(envelope.Event), envelope.EntityId);
                    break;

                case MessageType.UdalostCreated:
                      _repository.LastEventCheck(JsonConvert.DeserializeObject<EventUdalostCreated>(envelope.Event), envelope.EntityId);
                    break;
                case MessageType.UdalostUpdated:
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventUdalostUpdated>(envelope.Event), envelope.EntityId);
                    break;
                case MessageType.UdalostRemoved:
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventUdalostRemoved>(envelope.Event), envelope.EntityId);
                    break;
            }
        }






    }
}
