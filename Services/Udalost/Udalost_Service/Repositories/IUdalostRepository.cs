
using EventLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Udalost_Service.Entities;

namespace Udalost_Service.Repositories
{
    public interface IUdalostRepository
    {

        Task AddCommand(string message);
        Task Add(EventUdalostCreate message);
        Task Remove(EventUdalostRemove udalostId);
        Task Update(EventUdalostUpdate udalostId);
        //public IEnumerable<Udalost> GetList();
        //public Udalost Get(int bloId);

    }
}
