
using DochazkaLibrary.Models;
using EventLibrary;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace Dochazka_Service.Repositories
{
    public class DochazkaServiceRepository : IDochazkaServiceRepository
    {

        private string _connectionString;
        string _BaseUrl;



        public DochazkaServiceRepository(string connectionString)
        {
            _connectionString = connectionString;
            _BaseUrl = "http://dochazkaapi/dochazka/";

        }
        //-------------Description: Zpracování zpráv získaných po přihlášení k RabbitMQ Exchange
        public void AddCommand(string message)
        {
            //-------------Description: Deserializace Json objektu na základní typ zprávy
            var envelope = JsonConvert.DeserializeObject<Base>(message);


            //-------------Description: Rozhodnutí o typu získazné zprávy
            switch (envelope.MessageType)
            {
                case MessageType.DochazkaCreate:
                    //-------------Description: Kontrola verze zprávy 
                    if (envelope.Version == 1)
                    {
                        //-------------Description: Deserializace zprávy do správného typu a odeslání k uložení do DB; 
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
            //-------------Description: Vytvoření entity podle obdržené zprávy
            var model = new DochazkaModel()
            {
                Datum = cmd.Datum,
                Prichod = cmd.Prichod,
                CteckaId = cmd.CteckaId,
                UzivatelId = cmd.UzivatelId,
            };
            
            var client = new HttpClient();
            client.BaseAddress = new Uri(_BaseUrl);
            client.PostAsJsonAsync("Add", model);
        
        }
        public void Remove(CommandDochazkaRemove msg)
        {           
            var client = new HttpClient();
            client.BaseAddress = new Uri(_BaseUrl);
            client.DeleteAsync(string.Format("Remove/{0}", msg.Id));
        }
        public void Update(CommandDochazkaUpdate msg)
        {
            var model = new DochazkaModel()
            {
                Id = msg.Id,
                Datum = msg.Datum,
                Prichod = msg.Prichod,
                CteckaId = msg.CteckaId,
                UzivatelId = msg.UzivatelId,
            };
            var client = new HttpClient();
            client.BaseAddress = new Uri(_BaseUrl);
            client.PutAsJsonAsync("Update", model);           
        }
        





    }
}
