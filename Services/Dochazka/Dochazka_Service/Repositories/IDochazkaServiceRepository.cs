
using Dochazka_Service.Entities;
using EventLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Dochazka_Service.Repositories
{
    public interface IDochazkaServiceRepository
    {

        public void AddCommand(string message);
        public void Add(CommandDochazkaCreate message);      
        public void Remove(CommandDochazkaRemove message);
        public bool Update(CommandDochazkaUpdate message);
        //public IEnumerable<Dochazka> GetList();
        //public Dochazka Get(int bloId);


    }
}
