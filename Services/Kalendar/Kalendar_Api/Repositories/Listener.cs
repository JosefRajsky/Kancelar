

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
    public class Listener : IListener
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
            switch (envelope.MessageType)
            {
                case MessageType.KalendarCreate:
                   
                                    
                        this.AddAsync(JsonConvert.DeserializeObject<CommandKalendarCreate>(envelope.Event));
                  
                    break;              
                case MessageType.KalendarUpdate:
                  
                    
                        this.Update(JsonConvert.DeserializeObject<CommandKalendarUpdate>(envelope.Event));
                 
                    break;
                case MessageType.UdalostCreated:
                    
                        this.UpdateByUdalost(JsonConvert.DeserializeObject<EventUdalostCreated>(envelope.Event));
                 
                    break;
                //case MessageType.UzivatelCreated:
                                         
                //        this.AddByUzivatel(JsonConvert.DeserializeObject<EventUzivatelCreated>(envelope.Event));
                 
                //    break;
                //case MessageType.UzivatelUpdated:
                   
                //        this.UpdateWithUzivatel(JsonConvert.DeserializeObject<EventUzivatelUpdated>(envelope.Event));
                  
                //    break;
                //case MessageType.UzivatelRemoved:
                   
                //        this.DeleteWithUzivatel(JsonConvert.DeserializeObject<EventUzivatelDeleted>(envelope.Event));
                  
                //    break;
                default:
                    
                    break;
            }
        }

        public void AddAsync(CommandKalendarCreate cmd)
        {                                
            _repository.Add(cmd,false);
        }      
       
        public void Update(CommandKalendarUpdate cmd)
        {
            _repository.Update(cmd,false);             
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
