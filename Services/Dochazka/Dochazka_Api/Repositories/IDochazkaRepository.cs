
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
        Task<List<Dochazka>> GetList();
        Task<Dochazka> Get(string id);

        Task Add(DochazkaModel model);

        Task Update(DochazkaModel update);

        Task Remove(string id);
         

    }
}
