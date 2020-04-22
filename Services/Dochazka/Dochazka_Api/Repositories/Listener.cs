

using CommandHandler;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace Dochazka_Api.Repositories
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
                case MessageType.DochazkaCreated:

                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventDochazkaCreated>(envelope.Event).EventId, envelope.EntityId);
                    break;
                case MessageType.DochazkaUpdated:
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventDochazkaCreated>(envelope.Event).EventId, envelope.EntityId);
                    break;
                case MessageType.DochazkaDeleted:
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventDochazkaDeleted>(envelope.Event).EventId, envelope.EntityId);
                    break;
            }
        }
    }
}
