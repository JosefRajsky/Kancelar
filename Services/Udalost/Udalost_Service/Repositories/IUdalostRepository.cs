
using EventLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Udalost_Service.Entities;

namespace Udalost_Service.Repositories
{
    public interface IUdalostServiceRepository
    {

        Task AddCommand(string message);
        Task Add(CommandUdalostCreate message);
        Task Remove(CommandUdalostRemove udalostId);
        Task Update(CommandUdalostUpdate udalostId);
        //public IEnumerable<Udalost> GetList();
        //public Udalost Get(int bloId);

    }
}
