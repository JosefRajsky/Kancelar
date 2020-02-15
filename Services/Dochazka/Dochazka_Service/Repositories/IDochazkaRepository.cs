
using Dochazka_Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Dochazka_Service.Repositories
{
    public interface IDochazkaRepository
    {
        public IEnumerable<Dochazka> GetList();
        public Dochazka Get(int bloId);

        public Dochazka Add(Dochazka input);

        public bool Update(Dochazka update);

        public bool Delete(int blogId);
         

    }
}
