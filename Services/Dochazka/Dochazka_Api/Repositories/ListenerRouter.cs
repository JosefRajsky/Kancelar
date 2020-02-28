
using DochazkaLibrary.Models;
using EventLibrary;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace Dochazka_Api.Repositories
{
    public class ListenerRouter : IListenerRouter
    {
        //string _BaseUrl;
        private readonly IDochazkaRepository _dochazkaRepository;
        public ListenerRouter(IDochazkaRepository dochazkaService)
        {
            _dochazkaRepository = dochazkaService;
            //_BaseUrl = "http://dochazkaapi/dochazka/";
        }
       
        public void AddCommand(string message)
        {           
            var envelope = JsonConvert.DeserializeObject<Command>(message);          
            switch (envelope.MessageType)
            {
                case (int)MessageType.DochazkaCreate:
                   
                    if (envelope.Version == 1)
                    {                      
                        this.AddAsync(JsonConvert.DeserializeObject<CommandDochazkaCreate>(message));
                    }
                    break;
                case MessageType.DochazkaRemove:
                    if (envelope.Version == 1)
                    {
                        this.Remove(JsonConvert.DeserializeObject<CommandDochazkaRemove>(message));
                    }
                    break;
                case MessageType.DochazkaUpdate:
                    if (envelope.Version == 1)
                    {
                        this.Update(JsonConvert.DeserializeObject<CommandDochazkaUpdate>(message));
                    }
                    break;
              
                default:
                    
                    break;
            }
        }

        public void AddAsync(CommandDochazkaCreate cmd)
        {                        
        
            _dochazkaRepository.Add(cmd);
            //client.PutAsJsonAsync("Add", cmd);

        }
      
        public void Remove(CommandDochazkaRemove cmd)
        {           
           
            _dochazkaRepository.Remove(cmd);
          
        }
        public void Update(CommandDochazkaUpdate cmd)
        {
            _dochazkaRepository.Update(cmd);
             
        }
        





    }
}
