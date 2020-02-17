using KancelarWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.Interfaces
{
    public interface IDochazkaReader
    {
        Task<IEnumerable<DochazkaModel>> GetList();
        DochazkaModel Get(int id);
    
    }
}
