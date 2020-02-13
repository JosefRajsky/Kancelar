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

        bool Add(DochazkaModel dochazka);

        bool Delete(int id);
    }
}
