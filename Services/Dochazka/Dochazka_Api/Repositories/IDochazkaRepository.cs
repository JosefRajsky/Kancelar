
using Dochazka_Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Dochazka_Api.Repositories
{
    public interface IDochazkaRepository
    {
        public IEnumerable<Dochazka> GetList();
        public Dochazka Get(int bloId);

        public Task<bool> Add(DochazkaModel model);

        public Task Update(DochazkaModel update);

        public Task<bool> Delete(int Id);
         

    }
}
