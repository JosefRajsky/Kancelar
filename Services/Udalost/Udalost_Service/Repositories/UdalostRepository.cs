
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
        public async Task AddMessage(string message)
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
                        await this.Add(JsonConvert.DeserializeObject<EventUdalostCreate>(message));
                    }
                    break;
                case MessageType.DochazkaRemove:
                    if (envelope.Version == 1)
                    {
                        await this.Remove(JsonConvert.DeserializeObject<EventUdalostRemove>(message));
                    }
                    break;
               case MessageType.DochazkaUpdate:
                    if (envelope.Version == 1)
                    {
                        await this.Update(JsonConvert.DeserializeObject<EventUdalostUpdate>(message));
                    }
                    break;
      
            }
        }
        public async Task Add(EventUdalostCreate msg)
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
                await db.SaveChangesAsync();
            }          
        }
        public async Task Remove(EventUdalostRemove msg)
        {
            using (var db = new UdalostFactory(_connectionString).CreateDbContext())
            {
                var remove = db.Udalosti.FirstOrDefault(b => b.Id == msg.UdalostId);
                if (remove != null) {
                    db.Udalosti.Remove(remove);
                    await db.SaveChangesAsync();                    
                } 
            }
        }
        public async Task Update(EventUdalostUpdate msg)
        {
            using (var db = new UdalostFactory(_connectionString).CreateDbContext())
            {
                var forUpdate = db.Udalosti.FirstOrDefault(b => b.Id == msg.UdalostId);
                forUpdate.DatumOd = msg.DatumOd;
                forUpdate.DatumDo = msg.DatumDo;
                forUpdate.UdalostTypId = msg.UdalostTypId;
                forUpdate.Popis = msg.Popis;
                forUpdate.UzivatelId = msg.UzivatelId;
                forUpdate.DatumZadal = msg.DatumZadal;
                db.Udalosti.Update(forUpdate);
                await db.SaveChangesAsync();
                
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
