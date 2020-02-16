
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

        public void Add(EventDochazkaCreate msg)
        {
            //Description: Vytvoření entity podle obdržené zprávy
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
            //Description: Založení připojení k databázi
            using (var db = new DochazkaDbContextFactory(_connectionString).CreateDbContext())
            {
                //Description: Přidání a uložení do DB; Ukončení spojení
                db.Dochazka.Add(add);
                db.SaveChanges();
                db.Dispose();
            }   
        }
        public Dochazka Get(int id)
        {
            using (var db = new DochazkaDbContextFactory(_connectionString).CreateDbContext())
            {
                return db.Dochazka.FirstOrDefault(b => b.Id == id);
            }
            
        }
        public IEnumerable<Dochazka> GetList()
        {
            using (var db = new DochazkaDbContextFactory(_connectionString).CreateDbContext())
            {
                return db.Dochazka;
            }
           
        }
        public void Remove(EventDochazkaRemove msg)
        {
            using (var db = new DochazkaDbContextFactory(_connectionString).CreateDbContext())
            {
                var remove = db.Dochazka.FirstOrDefault(b => b.Id == msg.DochazkaId);
                db.Dochazka.Remove(remove);
                db.SaveChanges();
            }           
        }

        public bool Update(Dochazka update)
        {
            using (var db = new DochazkaDbContextFactory(_connectionString).CreateDbContext())
            {
            var forUpdate = db.Dochazka.FirstOrDefault(b => b.Id == update.Id);
            forUpdate = update;
            db.Dochazka.Update(forUpdate);
            db.SaveChanges();
            }           
            return true;
        }
        //Description: Zpracování zpráv získaných po přihlášení k RabbitMQ Exchange
        public void AddMessage(string message)
        {
            //Description: Deserializace Json objektu na základní typ zprávy
            var envelope = JsonConvert.DeserializeObject<EventBase>(message);
      

            //Description: Rozhodnutí o typu získazné zprávy
            switch (envelope.MessageType)
                {
                    case EventEnum.MessageType.DochazkaCreate:
                    //Description: Kontrola verze zprávy 
                    if (envelope.Version == 1) {
                        //Description: Deserializace zprávy do správného typu a odeslání k uložení do DB; 
                        this.Add(JsonConvert.DeserializeObject<EventDochazkaCreate>(message));
                    }                   
                    break;
                case EventEnum.MessageType.DochazkaRemove:
                    if (envelope.Version == 1)
                    {
                        this.Remove(JsonConvert.DeserializeObject<EventDochazkaRemove>(message));
                    }
                    break;
                default:
                        break;
                }

           

        }
    }
}
