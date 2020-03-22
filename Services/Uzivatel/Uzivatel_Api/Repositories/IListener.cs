
using EventLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uzivatel_Api.Repositories
{
    public interface IListener
    {

        void AddCommand(string message);
        void Add(CommandUzivatelCreate message, Guid replayed);
        void Remove(CommandUzivatelRemove id, Guid replayed);
        void Update(CommandUzivatelUpdate id, Guid replayed);


    }
}
