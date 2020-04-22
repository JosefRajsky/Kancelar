

using CommandHandler;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace Dochazka_Api.Repositories
{
    public class Listener
    {
        //string _BaseUrl;
        private readonly IRepository _repository;
        public Listener(IRepository repository)
        {
            _repository = repository;

            //CheckState();
        }
        
        public void AddCommand(string message)
        {
            var envelope = JsonConvert.DeserializeObject<Message>(message);
            switch (envelope.MessageType)
            {
                
                case MessageType.DochazkaCreate:
                                   
                        this.AddAsync(JsonConvert.DeserializeObject<CommandDochazkaCreate>(envelope.Event));
                  
                    break;
                case MessageType.DochazkaRemove:
                  
                        this.Remove(JsonConvert.DeserializeObject<CommandDochazkaRemove>(envelope.Event));
                   
                    break;
                case MessageType.DochazkaUpdate:
                    
                        this.Update(JsonConvert.DeserializeObject<CommandDochazkaUpdate>(envelope.Event));
                  
                    break;
              
                default:
                    
                    break;
            }
        }
        public void AddAsync(CommandDochazkaCreate cmd)
        {                                
            _repository.Add(cmd,false);
        }      
        public void Remove(CommandDochazkaRemove cmd)
        {                      
            _repository.Remove(cmd, false);          
        }
        public void Update(CommandDochazkaUpdate cmd)
        {
            _repository.Update(cmd, false);             
        }
        





    }
}
