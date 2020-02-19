using KancelarWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.Interfaces
{
    public interface IDochazkaProvider
    {
        Task<List<DochazkaModel>> GetList();
        T Get<T>(int id);
        Task Add<T>(T model);
        Task Update<T>(T model);
        Task Remove(int id);
    }
}
