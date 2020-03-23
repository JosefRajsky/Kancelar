
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
        void Restore(Message msg);
        void Remove(Message msg);
        void ReUpdate(Message msg);


    }
}
