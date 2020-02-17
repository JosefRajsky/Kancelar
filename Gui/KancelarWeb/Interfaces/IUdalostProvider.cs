using KancelarWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.Interfaces
{
    public interface IUdalostProvider
    {
        Task<IEnumerable<UdalostModel>> GetList();
        UdalostModel Get(int id);

        Task Add(UdalostModel udalost);

        Task Update(UdalostModel udalost);

        Task Remove(int id);
    }
}
