
using EventLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Udalost_Api.Repositories
{
    public interface IUdalostServiceRepository
    {

        void AddCommand(string message);
        void Add(CommandUdalostCreate message);
        void Remove(CommandUdalostRemove udalostId);
        void Update(CommandUdalostUpdate udalostId);
        //public IEnumerable<Udalost> GetList();
        //public Udalost Get(int bloId);

    }
}
