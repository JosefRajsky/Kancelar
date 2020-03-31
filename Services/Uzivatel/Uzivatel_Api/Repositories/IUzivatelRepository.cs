

using CommandHandler;
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
        Task Update(CommandUzivatelUpdate cmd);
        Task Remove(CommandUzivatelRemove cmd);
        Task ConfirmAdd(EventUzivatelCreated evt, Guid entityId);
        Task ConfirmUpdate(EventUzivatelUpdated evt, Guid entityId);
        Task ReplayStream(List<string> msgstream,Guid? entityId);
        Task RequestReplay(Guid? entityId);






    }
}
