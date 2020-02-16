using KancelarWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.Interfaces
{
    public interface IDochazkaProvider
    {
        Task<IEnumerable<DochazkaModel>> GetList();
        DochazkaModel Get(int id);
        Task<bool> Add(DochazkaModel dochazka);

        Task<bool> Delete(int id);
    }
}
