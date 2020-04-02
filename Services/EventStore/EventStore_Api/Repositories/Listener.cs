

using CommandHandler;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace EventStore.Repositories
{
    public class Listener 
    {
        //string _BaseUrl;
        private readonly IEventStoreRepository _repository;
        public Listener(IEventStoreRepository repository)
        {
            _repository = repository;
        }
        public void AddCommand(string message)
        {         
            var envelope = JsonConvert.DeserializeObject<Message>(message);
            switch (envelope.MessageType)
            {
                case MessageType.ProvideHealingStream:                   
                    _repository.ProvideHealingStream(message);
                    break;  
                default:
                    _repository.AddMessageAsync(message);
                    break;
            }
        }       
        





    }
}
