
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Udalost_Api.Entities;

namespace Udalost_Api.Repositories
{
    public interface IUdalostRepository
    {
        public IEnumerable<Udalost> List();
        public Udalost Get(int bloId);

        public Udalost Add(Udalost input);

        public bool Update(Udalost update);

        public bool Delete(int blogId);
         

    }
}
