
using DochazkaLibrary.Models;
using EventLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dochazka_Api.Repositories
{
    public interface IDochazkaRepository
    {
        Task<List<Dochazka>> GetList();
        Task<Dochazka> Get(string id);

        Task Add(CommandDochazkaCreate cmd);

        Task Update(CommandDochazkaUpdate cmd);

        Task Remove(CommandDochazkaRemove cmd);
         

    }
}
