
using CommandHandler;
using Newtonsoft.Json;
using System;

namespace Udalost_Api.Repositories
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
        public void AddCommand(string message) { 

            //-------------Description: Deserializace Json objektu na základní typ zprávy
        var envelope = JsonConvert.DeserializeObject<Message>(message);
            //-------------Description: Rozhodnutí o typu získazné zprávy. Typ vázaný na Enum z knihovny

            switch (envelope.MessageType)
            {
                case MessageType.UdalostCreated:
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventUdalostCreated>(envelope.Event).EventId, envelope.EntityId);
                    break;
                case MessageType.UdalostUpdated:
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventUdalostUpdated>(envelope.Event).EventId, envelope.EntityId);
                    break;
                case MessageType.UdalostRemoved:
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventUdalostRemoved>(envelope.Event).EventId, envelope.EntityId);
                    break;
            }
        }
  
       
        public void AddByDochazka(EventDochazkaCreated evt)
        {
            var cmd = new CommandUdalostCreate()
            {
                DatumOd = evt.Datum,
                DatumDo = evt.Datum.AddHours(8),
                Popis = string.Empty,
                UdalostTypId = 1,
                Nazev = "Přítomnost",
                UzivatelId = evt.UzivatelId,
                DatumZadal = evt.EventCreated
            };
            _repository.Add(cmd);
        }
       

    }
}
