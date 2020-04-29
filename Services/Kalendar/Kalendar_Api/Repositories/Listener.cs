


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
            //Description: Deserializace zprávy
            var envelope = JsonConvert.DeserializeObject<Message>(message);
            //Description: Kontrola typu zprávy a rozhodnutí o způsobu konzumace
            switch (envelope.MessageType)
            {
                case MessageType.HealingStreamProvided:
                  
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
                    RemoveByUzivatel(JsonConvert.DeserializeObject<EventUzivatelRemoved>(envelope.Event));
                    break;

                case MessageType.AktivitaCreated:
                    CreateByAktivita(JsonConvert.DeserializeObject<EventAktivitaCreated>(envelope.Event));
                    break;
                case MessageType.AktivitaUpdated:
                    UpdateByAktivita(JsonConvert.DeserializeObject<EventAktivitaUpdated>(envelope.Event));
                    break;
                case MessageType.AktivitaRemoved:
                    RemoveByAktivita(JsonConvert.DeserializeObject<EventAktivitaRemoved>(envelope.Event));
                    break;
            }
        }
        //Description: Reakce na událost vytvoření uživatele
        private void CreateByUzivatel(EventUzivatelCreated evt) {
            _repository.CreateByUzivatel(evt);
        }
        private void UpdateByUzivatel(EventUzivatelUpdated evt)
        {
            _repository.UpdateByUzivatel(evt);
        }
        private void RemoveByUzivatel(EventUzivatelRemoved evt)
        {
            _repository.DeleteByUzivatel(evt);
        }
        private void CreateByAktivita(EventAktivitaCreated evt)
        {
            _repository.CreateByAktivita(evt);
        }
        private void UpdateByAktivita(EventAktivitaUpdated evt)
        {
            _repository.UpdateByAktivita(evt);
        }
        private void RemoveByAktivita(EventAktivitaRemoved evt)
        {
            _repository.DeleteByAktivita(evt);
        }
        private void ReplayEvents(List<string> stream, Guid? entityId)
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
