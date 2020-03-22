
using CommandHandler;

using EventLibrary;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Udalost_Api.Models;
using UdalostLibrary;

namespace Udalost_Api.Repositories
{
    public class Listener : IListener
    {
        private readonly IUdalostRepository _repository;
        public Listener(IUdalostRepository repository)
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
           
            //-------------Description: Deserializace Json objektu na základní typ zprávy
            var envelope = JsonConvert.DeserializeObject<Message>(message);
            //-------------Description: Rozhodnutí o typu získazné zprávy. Typ vázaný na Enum z knihovny

            switch (envelope.MessageType)
            {

                case MessageType.UdalostRemove:
                   
                        this.Remove(JsonConvert.DeserializeObject<CommandUdalostRemove>(envelope.Event));
                   
                    break;
                case MessageType.UdalostUpdate:
                    
                        this.Update(JsonConvert.DeserializeObject<CommandUdalostUpdate>(envelope.Event));
                   
                    break;
                case MessageType.DochazkaCreated:

                        this.AddByDochazka(JsonConvert.DeserializeObject<EventDochazkaCreated>(envelope.Event));
                   
                    break;


            }
        }
  
        public void Add(CommandUdalostCreate cmd)
        {
            _repository.Add(cmd);
      
        }
        public void AddByDochazka(EventDochazkaCreated evt)
        {
            var cmd = new CommandUdalostCreate()
            {
                DatumOd = evt.Datum,
                DatumDo = evt.Datum.AddHours(8),
                Popis = string.Empty,
                UdalostTypId = 1,
                Nazev = "Přítomnost",
                UzivatelId = evt.UzivatelId,
            };
            _repository.Add(cmd);
        }
        public void Remove(CommandUdalostRemove cmd)
        {
            _repository.Remove(cmd);
        }
        public void Update(CommandUdalostUpdate cmd)
        {
            _repository.Update(cmd);
        }

    }
}
