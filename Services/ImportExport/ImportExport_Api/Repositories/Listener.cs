
using CommandHandler;
using Newtonsoft.Json;
using System;

namespace ImportExport_Api.Repositories
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
                case MessageType.ExportPritomnost:
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventUdalostCreated>(envelope.Event).EventId, envelope.EntityId);
                    break;
            }
        } 
    }
}
