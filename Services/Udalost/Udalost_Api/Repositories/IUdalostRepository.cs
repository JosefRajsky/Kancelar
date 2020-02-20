
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Udalost_Api.Entities;
using UdalostLibrary.Models;

namespace Udalost_Api.Repositories
{
    public interface IUdalostRepository
    {
        Task<List<Udalost>> GetList();
        Task<Udalost> Get(int id);

        Task Add(UdalostModel input);

        Task Update(UdalostModel update);

        Task Remove(string id);
         

    }
}
