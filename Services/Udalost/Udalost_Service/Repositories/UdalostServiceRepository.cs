
using CommandHandler;

using EventLibrary;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UdalostLibrary;
using UdalostLibrary.Models;

namespace Udalost_Service.Repositories
{
    public class UdalostServiceRepository : IUdalostServiceRepository
    {
        public string _BaseUrl;
        public UdalostServiceRepository()
        {
            _BaseUrl = "http://udalostapi/udalost/";
        }
        public void AddCommand(string message)
        {
            //-------------Description: Deserializace Json objektu na základní typ zprávy
            var envelope = JsonConvert.DeserializeObject<Command>(message);
            //-------------Description: Rozhodnutí o typu získazné zprávy. Typ vázaný na Enum z knihovny
            switch (envelope.MessageType)
            {
                case MessageType.UdalostCreate:
                    //-------------Description: Kontrola verze zprávy 
                    if (envelope.Version == 1)
                    {
                        //-------------Description: Deserializace zprávy do správného typu a odeslání k uložení do DB; 
                        this.Add(JsonConvert.DeserializeObject<CommandUdalostCreate>(message));
                    }
                    break;
                case MessageType.UdalostRemove:
                    if (envelope.Version == 1)
                    {
                        this.Remove(JsonConvert.DeserializeObject<CommandUdalostRemove>(message));
                    }
                    break;
                case MessageType.UdalostUpdate:
                    if (envelope.Version == 1)
                    {
                        this.Update(JsonConvert.DeserializeObject<CommandUdalostUpdate>(message));
                    }
                    break;
                case MessageType.DochazkaCreated:
                    if (envelope.Version == 1)
                    {
                        this.AddByDochazka(JsonConvert.DeserializeObject<EventDochazkaCreated>(message));
                    }
                    break;

            }
        }
        public void Add(CommandUdalostCreate cmd)
        {

            var client = new HttpClient();
            client.BaseAddress = new Uri(_BaseUrl);
            client.PutAsJsonAsync("Add", cmd);
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
            var client = new HttpClient();
            client.BaseAddress = new Uri(_BaseUrl);
            client.PutAsJsonAsync("Add", cmd);
        }
        public void Remove(CommandUdalostRemove cmd)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_BaseUrl);
            client.PostAsJsonAsync("Remove", cmd);
        }
        public void Update(CommandUdalostUpdate cmd)
        {
            var model = new UdalostModel()
            {
                DatumOd = cmd.DatumOd,
                DatumDo = cmd.DatumDo,
                Popis = cmd.Popis,
                UdalostTypId = cmd.UdalostTypId,
                UzivatelId = cmd.UzivatelId,
            };
            var client = new HttpClient();
            client.BaseAddress = new Uri(_BaseUrl);
            client.PostAsJsonAsync("Update", model);

        }
    }
}
