
using Dochazka_Service.Repositories;
using EventLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Udalost_Service.Entities;

namespace Udalost_Service.Repositories
{
    public class UdalostRepository : IUdalostRepository
    {
        private string _connectionString;


        public UdalostRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddMessage(string message)
        {
            //-------------Description: Deserializace Json objektu na základní typ zprávy
            var envelope = JsonConvert.DeserializeObject<EventBase>(message);
            //-------------Description: Rozhodnutí o typu získazné zprávy
            switch (envelope.MessageType)
            {
                case EventEnum.MessageType.DochazkaCreate:
                    //-------------Description: Kontrola verze zprávy 
                    if (envelope.Version == 1)
                    {
                        //-------------Description: Deserializace zprávy do správného typu a odeslání k uložení do DB; 
                        this.Add(JsonConvert.DeserializeObject<EventUdalostCreate>(message));
                    }
                    break;
                case EventEnum.MessageType.DochazkaRemove:
                    if (envelope.Version == 1)
                    {
                        this.Remove(JsonConvert.DeserializeObject<EventUdalostRemove>(message));
                    }
                    break;
               case EventEnum.MessageType.DochazkaUpdate:
                    if (envelope.Version == 1)
                    {
                        this.Update(JsonConvert.DeserializeObject<EventUdalostUpdate>(message));
                    }
                    break;
                default:
                    break;
            }
        }
        public void Add(EventUdalostCreate msg)
        {
            using (var db = new UdalostFactory(_connectionString).CreateDbContext())
            {
                var add = new Udalost()
                {
                    DatumOd = msg.DatumOd,
                    DatumDo = msg.DatumDo,                    
                    Nazev = msg.Nazev,
                    UzivatelId = msg.UzivatelId,
                    DatumZadal = msg.DatumZadal,
                };                
                db.Add(add);
                db.SaveChanges();
            }          
        }
        public void Remove(EventUdalostRemove msg)
        {
            using (var db = new UdalostFactory(_connectionString).CreateDbContext())
            {
                var remove = db.Udalosti.FirstOrDefault(b => b.Id == msg.UdalostId);
                db.Udalosti.Remove(remove);
                db.SaveChanges();
            }
        }
        public bool Update(EventUdalostUpdate msg)
        {
            using (var db = new UdalostFactory(_connectionString).CreateDbContext())
            {
                var forUpdate = db.Udalosti.FirstOrDefault(b => b.Id == msg.UdalostId);
                forUpdate.DatumOd = msg.DatumOd;
                forUpdate.DatumDo = msg.DatumDo;
                forUpdate.Nazev = msg.Nazev;
                forUpdate.UzivatelId = msg.UzivatelId;
                forUpdate.DatumZadal = msg.DatumZadal;
                db.Udalosti.Update(forUpdate);
                db.SaveChanges();
                return true;
            }

        }
        //public Udalost Get(int id)
        //{
        //    using (var db = new UdalostFactory(_connectionString).CreateDbContext())
        //    {
        //        return db.Udalosti.FirstOrDefault(b => b.Id == id);
        //    }
        //}
        //public IEnumerable<Udalost> GetList()
        //{
        //    using (var db = new UdalostFactory(_connectionString).CreateDbContext())
        //    {
        //        return db.Udalosti;
        //    }            
        //}



    }
}
