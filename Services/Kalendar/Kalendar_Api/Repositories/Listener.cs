

using EventLibrary;
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
    public class Listener : IListenerRouter
    {
        //string _BaseUrl;
        private readonly IKalendarRepository _repository;
        public Listener(IKalendarRepository repository)
        {
            _repository = repository;
        }
       
        public void AddCommand(string message)
        {
            var envelope = JsonConvert.DeserializeObject<Message>(message);
            var body = JsonConvert.DeserializeObject<string>(envelope.Body);
            switch (envelope.MessageType)
            {
                case MessageType.KalendarCreate:
                   
                    if (envelope.Version == 1)
                    {                      
                        this.AddAsync(JsonConvert.DeserializeObject<CommandKalendarCreate>(body));
                    }
                    break;              
                case MessageType.KalendarUpdate:
                    if (envelope.Version == 1)
                    {
                        this.Update(JsonConvert.DeserializeObject<CommandKalendarUpdate>(body));
                    }
                    break;
                case MessageType.UdalostCreated:
                    if (envelope.Version == 1) {
                        this.UpdateByUdalost(JsonConvert.DeserializeObject<EventUdalostCreated>(body));
                    }
                    break;
                case MessageType.UzivatelCreated:
                    if (envelope.Version == 1)
                    {                       
                        this.AddByUzivatel(JsonConvert.DeserializeObject<EventUzivatelCreated>(body));
                    }
                    break;
                case MessageType.UzivatelUpdated:
                    if (envelope.Version == 1)
                    {
                        this.UpdateWithUzivatel(JsonConvert.DeserializeObject<EventUzivatelUpdated>(body));
                    }
                    break;
                case MessageType.UzivatelRemoved:
                    if (envelope.Version == 1)
                    {
                        this.DeleteWithUzivatel(JsonConvert.DeserializeObject<EventUzivatelDeleted>(body));
                    }
                    break;
                default:
                    
                    break;
            }
        }

        public void AddAsync(CommandKalendarCreate cmd)
        {                                
            _repository.Add(cmd);
        }      
       
        public void Update(CommandKalendarUpdate cmd)
        {
            _repository.Update(cmd);             
        }
        public void UpdateByUdalost(EventUdalostCreated evt)
        {
            _repository.UpdateByUdalost(evt);
        }
        public void AddByUzivatel(EventUzivatelCreated evt)
        {
            _repository.AddByUzivatel(evt);
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
