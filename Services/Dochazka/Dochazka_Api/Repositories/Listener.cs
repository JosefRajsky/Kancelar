

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
    public class Listener : IListener
    {
        //string _BaseUrl;
        private readonly IDochazkaRepository _repository;
        public Listener(IDochazkaRepository repository)
        {
            _repository = repository;

            //CheckState();
        }
        public async void CheckState()
        {
            var _BaseUrl = "http://eventstore/eventstore/";
            var client = new HttpClient();
            client.BaseAddress = new Uri(_BaseUrl);
            var result = new HttpResponseMessage();
            result = await client.GetAsync("GetList");
            if (result.IsSuccessStatusCode)
            {
                var items = result.Content.ReadAsAsync<List<Message>>().Result;
                foreach (var item in items)
                {
                    AddCommand(JsonConvert.SerializeObject(item));
                }
            }
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
