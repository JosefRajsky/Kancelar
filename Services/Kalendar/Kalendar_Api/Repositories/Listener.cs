


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
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventUzivatelCreated>(envelope.Event), envelope.EntityId);
                    break;
                case MessageType.UzivatelUpdated:
                    _repository.LastEventCheck(JsonConvert.DeserializeObject<EventUzivatelCreated>(envelope.Event), envelope.EntityId);
                    break;
            }

            //var envelope = JsonConvert.DeserializeObject<Message>(message);
            //switch (envelope.MessageType)
            //{
            //    case MessageType.KalendarCreate:
            //            this.AddAsync(JsonConvert.DeserializeObject<CommandKalendarCreate>(envelope.Event));                  
            //        break;              
            //    case MessageType.KalendarUpdate:            
            //            this.Update(JsonConvert.DeserializeObject<CommandKalendarUpdate>(envelope.Event));                 
            //        break;
            //    case MessageType.UdalostCreated:                    
            //            this.UpdateByUdalost(JsonConvert.DeserializeObject<EventUdalostCreated>(envelope.Event));                 
            //        break;
            //    case MessageType.UzivatelCreated:
            //        this.AddByUzivatel(JsonConvert.DeserializeObject<EventUzivatelCreated>(envelope.Event));
            //        break;              
            //    default:
                    
            //        break;
            //}
        }

        public void UpdateByUdalost(EventUdalostCreated evt)
        {
            _repository.UpdateByUdalost(evt);
        }
        public void AddByUzivatel(EventUzivatelCreated evt)
        {
            _repository.CreateByUzivatel(evt);
        }
        public void UpdateWithUzivatel(EventUzivatelUpdated evt)
        {
            _repository.UpdateByUzivatel(evt);
        }
        public void DeleteWithUzivatel(EventUzivatelDeleted evt) {
            _repository.DeleteByUzivatel(evt);
        }






    }
}
