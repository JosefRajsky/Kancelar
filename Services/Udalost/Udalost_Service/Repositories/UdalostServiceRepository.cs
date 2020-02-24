
using CommandHandler;
using Dochazka_Service.Repositories;
using EventLibrary;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Udalost_Service.Entities;
using UdalostLibrary.Models;

namespace Udalost_Service.Repositories
{
    public class UdalostServiceRepository : IUdalostServiceRepository
    {
        private string _connectionString;
      


        public UdalostServiceRepository(string connectionString)
        {
            _connectionString = connectionString;
           
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
        public void Add(CommandUdalostCreate msg)
        {
            using (var db = new UdalostFactory(_connectionString).CreateDbContext())
            {
                var add = new Udalost()
                {
                    DatumOd = msg.DatumOd,
                    DatumDo = msg.DatumDo,        
                    UdalostTypId = msg.UdalostTypId,
                    Popis = msg.Popis,
                    UzivatelId = msg.UzivatelId,
                    DatumZadal = msg.DatumZadal,
                };                
                db.Add(add);
                db.SaveChanges();
               

            }          
        }
        public void Remove(CommandUdalostRemove msg)
        {
            using (var db = new UdalostFactory(_connectionString).CreateDbContext())
            {
                var remove = db.Udalosti.FirstOrDefault(b => b.Id == msg.UdalostId);
                if (remove != null) {
                    db.Udalosti.Remove(remove);
                    db.SaveChanges();
                   
                } 
            }
        }
        public void Update(CommandUdalostUpdate msg)
        {
            using (var db = new UdalostFactory(_connectionString).CreateDbContext())
            {
                var update = db.Udalosti.FirstOrDefault(b => b.Id == msg.UdalostId);
                update.DatumOd = msg.DatumOd;
                update.DatumDo = msg.DatumDo;
                update.UdalostTypId = msg.UdalostTypId;
                update.Popis = msg.Popis;
                update.UzivatelId = msg.UzivatelId;
                update.DatumZadal = msg.DatumZadal;
                db.Udalosti.Update(update);

                var result = db.SaveChanges();
                
            }

        }




    }
}
