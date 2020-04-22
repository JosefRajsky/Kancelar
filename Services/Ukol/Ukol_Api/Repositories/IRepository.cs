using CommandHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ukol_Api.Repositories
{
    public interface IRepository
    {
        Task<List<Ukol>> GetList();
        Task<Ukol> Get(Guid id);
        Task Add(CommandUkolCreate cmd);
        Task Update(CommandUkolUpdate cmd);
        Task Remove(CommandUkolRemove cmd);       
        Task LastEventCheck(Guid eventId, Guid entityId);
        Task ReplayEvents(List<string> msgstream,Guid? entityId);
        Task RequestEvents(Guid? entityId);






    }
}
