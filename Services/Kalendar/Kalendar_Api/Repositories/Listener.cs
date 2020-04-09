


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
                    ReplayEvents(ev.MessageList, envelope.EntityId);
                    break;
                case MessageType.KalendarCreated:
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventKalendarCreated>(envelope.Event).EventId, envelope.EntityId);
                    break;
                case MessageType.KalendarUpdated:

                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventKalendarUpdated>(envelope.Event).EventId, envelope.EntityId);
                    break;
               

                case MessageType.UzivatelCreated:
                    CreateByUzivatel(JsonConvert.DeserializeObject<EventUzivatelCreated>(envelope.Event));
                    break;
                case MessageType.UzivatelUpdated:
                   
                    UpdateByUzivatel(JsonConvert.DeserializeObject<EventUzivatelUpdated>(envelope.Event));
                    break;
                case MessageType.UzivatelRemoved:
                    RemoveByUzivatel(JsonConvert.DeserializeObject<EventUzivatelDeleted>(envelope.Event));
                    break;

                //case MessageType.UdalostCreated:
                //      _repository.LastEventCheck(JsonConvert.DeserializeObject<EventUdalostCreated>(envelope.Event), envelope.EntityId);
                //    break;
                //case MessageType.UdalostUpdated:
                //    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventUdalostUpdated>(envelope.Event), envelope.EntityId);
                //    break;
                //case MessageType.UdalostRemoved:
                //    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventUdalostRemoved>(envelope.Event), envelope.EntityId);
                //    break;
            }
        }

        private void CreateByUzivatel(EventUzivatelCreated evt) {
            _repository.CreateByUzivatel(evt);
        }
        private void UpdateByUzivatel(EventUzivatelUpdated evt)
        {
            _repository.UpdateByUzivatel(evt);
        }
        private void RemoveByUzivatel(EventUzivatelDeleted evt)
        {
            _repository.DeleteByUzivatel(evt);
        }
        public void ReplayEvents(List<string> stream, Guid? entityId)
        {
            var messages = new List<Message>();
            foreach (var item in stream)
            {
                messages.Add(JsonConvert.DeserializeObject<Message>(item));
            }
            var replayOrderedStream = messages.OrderBy(d => d.Created);
            foreach (var msg in replayOrderedStream)
            {
                AddCommand(JsonConvert.SerializeObject(msg));
            }           
        }



    }
}
