
using Dochazka_Service.Entities;
using EventLibrary;
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
        private readonly DochazkaDbContext db;
        public DochazkaRepository(DochazkaDbContext dochazkaDbContext) {
            db = dochazkaDbContext;           
        }

        public DochazkaRepository()
        {
        }

        public Dochazka Add(Dochazka input)
        {
            var add = new Dochazka();
            add = input;
            db.Dochazka.Add(add);
            db.SaveChanges();
            return add;
        }
        public Dochazka Get(int id)
        {
           return db.Dochazka.FirstOrDefault(b => b.Id == id);
        }
        public IEnumerable<Dochazka> GetList()
        {
            return db.Dochazka;
        }
        public bool Delete(int id)
        {
            var remove = db.Dochazka.FirstOrDefault(b => b.Id == id);
            db.Dochazka.Remove(remove);
            db.SaveChanges();
            return true;
        }

        public bool Update(Dochazka update)
        {
            var forUpdate = db.Dochazka.FirstOrDefault(b => b.Id == update.Id);
            forUpdate = update;
            db.Dochazka.Update(forUpdate);
            db.SaveChanges();
            return true;
        }

        public void AddMessage(string message)
        {
            var msg = JsonConvert.DeserializeObject<EventDochazkaCreate>(message);
            var add = new Dochazka()
            {
                Den =  msg.Datum.Day,
                DenTydne = (int) msg.Datum.DayOfWeek,
                Mesic =msg.Datum.Month,
                Rok = msg.Datum.Year,
                Prichod =msg.Prichod,
                Tick = msg.Datum.Ticks,
                UzivatelId = msg.UzivatelId,
               
            };            
           
            db.Dochazka.Add(add);
            db.SaveChanges();
            //return add;
        }
    }
}
