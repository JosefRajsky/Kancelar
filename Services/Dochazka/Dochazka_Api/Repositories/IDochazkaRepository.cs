
using Dochazka_Api.Entities;
using DochazkaLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Dochazka_Api.Repositories
{
    public interface IDochazkaRepository
    {
        public List<Dochazka> GetList();
        public Dochazka Get(string id);

        public Task Add(DochazkaModel model);

        public Task Update(DochazkaModel update);

        public Task Remove(string id);
         

    }
}
