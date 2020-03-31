
using Dochazka_Api.Models;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dochazka_Api.Repositories
{
    public interface IDochazkaRepository
    {
        Task<List<Dochazka>> GetList();
        Task<Dochazka> Get(Guid id);

        Task Add(CommandDochazkaCreate cmd, bool publish);

        Task Update(CommandDochazkaUpdate cmd, bool publish);

        Task Remove(CommandDochazkaRemove cmd, bool publish);
         

    }
}
