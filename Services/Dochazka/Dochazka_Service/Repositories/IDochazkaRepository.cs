
using Dochazka_Service.Entities;
using EventLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Dochazka_Service.Repositories
{
    public interface IDochazkaRepository
    {
        public IEnumerable<Dochazka> GetList();
        public Dochazka Get(int bloId);

        public void Add(EventDochazkaCreate message);

        public bool Update(Dochazka update);

        public void Remove(EventDochazkaRemove message);
         

    }
}
