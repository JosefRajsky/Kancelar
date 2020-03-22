
using EventLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uzivatel_Api.Repositories
{
    public interface IUzivatelRepository
    {
        Task<List<Uzivatel>> GetList();
        Task<Uzivatel> Get(Guid id);
        Task AddGeneration(Guid id);
        Task Add(CommandUzivatelCreate cmd, Guid? replayed);

        Task Update(CommandUzivatelUpdate cmd, Guid? replayed);

        Task Remove(CommandUzivatelRemove cmd, Guid? replayed);


    }
}
