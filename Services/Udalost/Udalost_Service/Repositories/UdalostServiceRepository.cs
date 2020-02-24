﻿
using CommandHandler;

using EventLibrary;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
            var envelope = JsonConvert.DeserializeObject<Base>(message);
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

            }
        }
        public void Add(CommandUdalostCreate cmd)
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
            client.PostAsJsonAsync("Add", model);
        }
        public void Remove(CommandUdalostRemove cmd)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(_BaseUrl);
            client.DeleteAsync(string.Format("Remove/{0}", cmd.Id));
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
