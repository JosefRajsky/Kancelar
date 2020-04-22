
using CommandHandler;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Nastaveni_Api.Repositories
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
            var envelope = JsonConvert.DeserializeObject<Message>(message);
            switch (envelope.MessageType)
            {
                case MessageType.HealingStreamProvided:
                    var ev = JsonConvert.DeserializeObject<HealingStreamProvided>(envelope.Event);
                    _repository.ReplayEvents(ev.MessageList, envelope.EntityId);
                    break;
                case MessageType.NastaveniCreated:

                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventNastaveniCreated>(envelope.Event).EventId, envelope.EntityId);
                    break;
                case MessageType.NastaveniUpdated:
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventNastaveniCreated>(envelope.Event).EventId, envelope.EntityId);
                    break;      
            }
        }
    }
}
