
using Web_Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Web_Api.Repositories
{
    public interface IApiDochazkaRepository
    {
        public IEnumerable<Dochazka> GetList();
        public Dochazka Get(string id);

        public Task Add(DochazkaModel model);

        public Task Update(DochazkaModel update);

        public Task Remove(string id);
         

    }
}
