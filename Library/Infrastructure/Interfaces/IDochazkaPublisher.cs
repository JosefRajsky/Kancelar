
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.Interfaces
{
    public interface IDochazkaPublisher
    {      
        Task<bool> Add<T>();

        Task<bool> Delete(int id);

        Task<bool> Update(DochazkaModel dochazka);
    }
}
