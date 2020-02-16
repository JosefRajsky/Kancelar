
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

        public void AddMessage(string message);
        public void Add(EventDochazkaCreate message);      
        public void Remove(EventDochazkaRemove message);
        public bool Update(EventDochazkaUpdate message);
        //public IEnumerable<Dochazka> GetList();
        //public Dochazka Get(int bloId);


    }
}
