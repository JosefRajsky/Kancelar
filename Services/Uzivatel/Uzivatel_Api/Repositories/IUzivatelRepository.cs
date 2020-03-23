
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
        Task Add(CommandUzivatelCreate cmd);
        Task Restore(CommandUzivatelCreate cmd, Guid entityId);

        Task ReUpdate(CommandUzivatelUpdate cmd, Guid entityId);

        Task Remove(CommandUzivatelRemove cmd, Guid entityId);

        Task Remove(CommandUzivatelRemove cmd);

        Task Update(CommandUzivatelUpdate cmd);


    }
}
