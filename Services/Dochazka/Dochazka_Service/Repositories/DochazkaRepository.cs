
using Dochazka_Service.Entities;
using EventLibrary;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Udalost_Service;

namespace Dochazka_Service.Repositories
{
    public class DochazkaRepository : IDochazkaRepository
    {

        private string _connectionString;
     

        public DochazkaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        //-------------Description: Zpracování zpráv získaných po přihlášení k RabbitMQ Exchange
        public void AddMessage(string message)
        {
            //-------------Description: Deserializace Json objektu na základní typ zprávy
            var envelope = JsonConvert.DeserializeObject<EventBase>(message);


            //-------------Description: Rozhodnutí o typu získazné zprávy
            switch (envelope.MessageType)
            {
                case MessageType.DochazkaCreate:
                    //-------------Description: Kontrola verze zprávy 
                    if (envelope.Version == 1)
                    {
                        //-------------Description: Deserializace zprávy do správného typu a odeslání k uložení do DB; 
                        this.Add(JsonConvert.DeserializeObject<EventDochazkaCreate>(message));
                    }
                    break;
                case MessageType.DochazkaRemove:
                    if (envelope.Version == 1)
                    {
                        this.Remove(JsonConvert.DeserializeObject<EventDochazkaRemove>(message));
                    }
                    break;
                case MessageType.DochazkaUpdate:
                    if (envelope.Version == 1)
                    {
                        this.Update(JsonConvert.DeserializeObject<EventDochazkaUpdate>(message));
                    }
                    break;
                default:
                    
                    break;
            }
        }

        public void Add(EventDochazkaCreate msg)
        {
            //-------------Description: Vytvoření entity podle obdržené zprávy
            var add = new Dochazka()
            {
                Den = msg.Datum.Day,
                DenTydne = (int)msg.Datum.DayOfWeek,
                Mesic = msg.Datum.Month,
                Rok = msg.Datum.Year,
                Prichod = msg.Prichod,
                Tick = msg.Datum.Ticks,
                UzivatelId = msg.UzivatelId,
            };
            //-------------Description: Založení připojení k databázi
            using (var db = new DochazkaDbContextFactory(_connectionString).CreateDbContext())
            {
                //-------------Description: Přidání a uložení do DB; Ukončení spojení
                db.Dochazka.Add(add);
                db.SaveChanges();
                db.Dispose();
            }   
        }
        public void Remove(EventDochazkaRemove msg)
        {
            using (var db = new DochazkaDbContextFactory(_connectionString).CreateDbContext())
            {
                var remove = db.Dochazka.FirstOrDefault(b => b.Id == msg.DochazkaId);
                if (remove != null) {
                    db.Dochazka.Remove(remove);
                    db.SaveChanges();
                }
  
            }           
        }
        public bool Update(EventDochazkaUpdate msg)
        {
            using (var db = new DochazkaDbContextFactory(_connectionString).CreateDbContext())
            {
                var forUpdate = db.Dochazka.FirstOrDefault(b => b.Id == msg.DochazkaId);
                if (forUpdate != null) {
                    forUpdate.Den = msg.Datum.Day;
                    forUpdate.DenTydne = (int)msg.Datum.DayOfWeek;
                    forUpdate.Mesic = msg.Datum.Month;
                    forUpdate.Rok = msg.Datum.Year;
                    forUpdate.Prichod = msg.Prichod;
                    forUpdate.Tick = msg.Datum.Ticks;
                    forUpdate.UzivatelId = msg.UzivatelId;
                    db.Dochazka.Update(forUpdate);
                    db.SaveChanges();
                    return true;
                }
             
            }
            return false;
        }
        //public Dochazka Get(int id)
        //{
        //    using (var db = new DochazkaDbContextFactory(_connectionString).CreateDbContext())
        //    {
        //        return db.Dochazka.FirstOrDefault(b => b.Id == id);
        //    }

        //}
        //public IEnumerable<Dochazka> GetList()
        //{
        //    using (var db = new DochazkaDbContextFactory(_connectionString).CreateDbContext())
        //    {
        //        return db.Dochazka;
        //    }

        //}





    }
}
