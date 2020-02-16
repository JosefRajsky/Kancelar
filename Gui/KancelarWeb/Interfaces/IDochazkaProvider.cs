using KancelarWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.Interfaces
{
    public interface IDochazkaProvider
    {
        IEnumerable<DochazkaModel> GetList();
        DochazkaModel Get(int id);
        Task Add(DochazkaModel dochazka);

        Task Delete(int id);
    }
}
