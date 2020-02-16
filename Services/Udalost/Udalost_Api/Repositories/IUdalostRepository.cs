
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Udalost_Api.Entities;
using Udalost_Api.Models;

namespace Udalost_Api.Repositories
{
    public interface IUdalostRepository
    {
        public IEnumerable<Udalost> GetList();
        public Udalost Get(int bloId);

        public Task Add(UdalostModel input);

        public Task Update(UdalostModel update);

        public Task Delete(int blogId);
         

    }
}
