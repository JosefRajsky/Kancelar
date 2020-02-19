
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web_Api.Entities;
using Web_Api.Models;

namespace Web_Api.Repositories
{
    public interface IApiUdalostRepository
    {
        public IEnumerable<Udalost> GetList();
        public Udalost Get(int id);

        public Task Add(UdalostModel input);

        public Task Update(UdalostModel update);

        public Task Delete(int id);
         

    }
}
