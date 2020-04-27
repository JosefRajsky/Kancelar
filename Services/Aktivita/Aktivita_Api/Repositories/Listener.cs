
using CommandHandler;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Aktivita_Api.Repositories
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
                case MessageType.AktivitaCreated:
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventAktivitaCreated>(envelope.Event).EventId, envelope.EntityId);
                    break;
                case MessageType.AktivitaUpdated:
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventAktivitaUpdated>(envelope.Event).EventId, envelope.EntityId);
                    break;
                case MessageType.AktivitaRemoved:
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventAktivitaRemoved>(envelope.Event).EventId, envelope.EntityId);
                    break;
            }
        }


        public void AddByDochazka(EventDochazkaCreated evt)
        {
            var cmd = new CommandAktivitaCreate()
            {
                DatumOd = evt.Datum,
                DatumDo = evt.Datum.AddHours(8),
                Popis = string.Empty,
                AktivitaTypId = 1,
                Nazev = "Přítomnost",
                UzivatelId = evt.UzivatelId,
                DatumZadal = evt.EventCreated
            };
            _repository.Add(cmd);
        }


    }
}
