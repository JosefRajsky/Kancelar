
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uzivatel_Api.Repositories
{
    public interface IUzivatelRepository
    {
        Task<List<Uzivatel>> GetList();
        Task<Uzivatel> Get(string id);

        Task Add(UzivatelModel model);

        Task Update(UzivatelModel update);

        Task Remove(int id);
         

    }
}
