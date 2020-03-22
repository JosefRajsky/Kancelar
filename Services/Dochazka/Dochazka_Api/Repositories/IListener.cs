

using EventLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Dochazka_Api.Repositories
{
    public interface IListener
    {
        void AddCommand(string message);
        void AddAsync(CommandDochazkaCreate message);      
        void Remove(CommandDochazkaRemove message);
        void Update(CommandDochazkaUpdate message);
    }
}
