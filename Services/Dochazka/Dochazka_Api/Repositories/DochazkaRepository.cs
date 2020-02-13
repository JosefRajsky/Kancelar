
using Dochazka_Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Udalost_Api;

namespace Dochazka_Api.Repositories
{
    public class DochazkaRepository : IDochazkaRepository
    {
        private readonly DochazkaDbContext db;
        public DochazkaRepository(DochazkaDbContext dochazkaDbContext) {
            db = dochazkaDbContext;
           
        }
        public Dochazka Add(Dochazka input)
        {
            var add = new Dochazka();
            add = input;
            db.Add(add);
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

 
    }
}
