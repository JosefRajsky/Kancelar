
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
            var envelope = JsonConvert.DeserializeObject<Message>(message);
            var body = JsonConvert.DeserializeObject<string>(envelope.Event);
            switch (envelope.MessageType)
            {
                case MessageType.DochazkaCreate:
                   
                                    
                        this.AddAsync(JsonConvert.DeserializeObject<CommandDochazkaCreate>(envelope.Command));
                 
                    break;
                case MessageType.DochazkaRemove:
                   
                        this.Remove(JsonConvert.DeserializeObject<CommandDochazkaRemove>(envelope.Command));
                   
                    break;
                case MessageType.DochazkaUpdate:
                    
                        this.Update(JsonConvert.DeserializeObject<CommandDochazkaUpdate>(envelope.Command));
                   
                    break;
              
                default:
                    
                    break;
            }
        }

        public void AddAsync(CommandDochazkaCreate cmd)
        {                        
        
            _dochazkaRepository.Add(cmd,false);
            //client.PutAsJsonAsync("Add", cmd);

        }
      
        public void Remove(CommandDochazkaRemove cmd)
        {           
           
            _dochazkaRepository.Remove(cmd, false);
          
        }
        public void Update(CommandDochazkaUpdate cmd)
        {
            _dochazkaRepository.Update(cmd, false);
             
        }
        





    }
}
