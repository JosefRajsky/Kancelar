

using EventLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Dochazka_Service.Repositories
{
    public interface IDochazkaServiceRepository
    {

        void AddCommand(string message);
        void AddAsync(CommandDochazkaCreate message);      
        void Remove(CommandDochazkaRemove message);
        void Update(CommandDochazkaUpdate message);
        //public IEnumerable<Dochazka> GetList();
        //public Dochazka Get(int bloId);


    }
}
