


using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Dochazka_Api.Repositories
{
    public interface IListenerRouter
    {

        void AddCommand(string message);
        void AddAsync(CommandDochazkaCreate message);      
        void Remove(CommandDochazkaRemove message);
        void Update(CommandDochazkaUpdate message);
        //public IEnumerable<Dochazka> GetList();
        //public Dochazka Get(int bloId);


    }
}
