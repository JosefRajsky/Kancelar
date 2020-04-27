

using CommandHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Uzivatel_Api.Repositories
{
    public interface IRepository
    {   
        Task<List<Uzivatel>> GetList();
        Task<Uzivatel> Get(Guid id);
        Task Add(CommandUzivatelCreate cmd);
        Task Update(CommandUzivatelUpdate cmd);
        Task Remove(CommandUzivatelRemove cmd);       
        Task LastEventCheck(Guid eventId, Guid entityId);
        Task ReplayEvents(List<string> msgstream,Guid? entityId);
        Task RequestEvents(Guid? entityId);
    }
}
