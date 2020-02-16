
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

        public void AddMessage(string message);
        public void Add(EventUdalostCreate message);
        public void Remove(EventUdalostRemove udalostId);
        public bool Update(EventUdalostUpdate udalostId);
        //public IEnumerable<Udalost> GetList();
        //public Udalost Get(int bloId);

    }
}
