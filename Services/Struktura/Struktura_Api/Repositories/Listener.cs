
using CommandHandler;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Struktura_Api.Repositories
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
                    CreateByUzivatel(JsonConvert.DeserializeObject<EventUzivatelCreated>(envelope.Event));
                    break;
                case MessageType.UzivatelUpdated:

                    UpdateByUzivatel(JsonConvert.DeserializeObject<EventUzivatelUpdated>(envelope.Event));
                    break;
                case MessageType.UzivatelRemoved:
                    RemoveByUzivatel(JsonConvert.DeserializeObject<EventUzivatelRemoved>(envelope.Event));
                    break;
                case MessageType.SoucastCreated:
                    CreateBySoucast(JsonConvert.DeserializeObject<EventSoucastCreated>(envelope.Event));
                    break;
                case MessageType.SoucastUpdated:

                    UpdateBySoucast(JsonConvert.DeserializeObject<EventSoucastUpdated>(envelope.Event));
                    break;
                case MessageType.SoucastRemoved:
                    RemoveBySoucast(JsonConvert.DeserializeObject<EventSoucastRemoved>(envelope.Event));
                    break;
            }
        }
        private void CreateByUzivatel(EventUzivatelCreated evt)
        {
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
        private void CreateBySoucast(EventSoucastCreated evt)
        {
            _repository.CreateBySoucast(evt);
        }
        private void UpdateBySoucast(EventSoucastUpdated evt)
        {
            _repository.UpdateBySoucast(evt);
        }
        private void RemoveBySoucast(EventSoucastRemoved evt)
        {
            _repository.DeleteBySoucast(evt);
        }



    }
}
