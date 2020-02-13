
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Udalost_Api.Entities;

namespace Udalost_Api.Repositories
{
    public class UdalostRepository : IUdalostRepository
    {
        private readonly UdalostDbContext db;
        public UdalostRepository(UdalostDbContext udalostDbContext) {
            db = udalostDbContext;
           
        }

        public Udalost Add(Udalost input)
        {
            var newUdalost = new Udalost();
            newUdalost = input;
            db.Add(newUdalost);
            db.SaveChanges();
            return newUdalost;
        }

        public Udalost Get(int id)
        {
           return db.Udalosti.FirstOrDefault(b => b.Id == id);
        }

        public IEnumerable<Udalost> GetList()
        {
            return db.Udalosti;
        }

        public bool Delete(int id)
        {
            var remove = db.Udalosti.FirstOrDefault(b => b.Id == id);
            db.Udalosti.Remove(remove);
            db.SaveChanges();
            return true;
        }

        public bool Update(Udalost update)
        {
            var forUpdate = db.Udalosti.FirstOrDefault(b => b.Id == update.Id);
            forUpdate = update;
            db.Udalosti.Update(forUpdate);
            db.SaveChanges();
            return true;
        }

 
    }
}
