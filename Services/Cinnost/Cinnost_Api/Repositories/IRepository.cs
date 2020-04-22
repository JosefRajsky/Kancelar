using CommandHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cinnost_Api.Repositories
{
    public interface IRepository
    {
        Task<List<Cinnost>> GetList();
        Task<Cinnost> Get(Guid id);
        Task Add(CommandCinnostCreate cmd);
        Task Update(CommandCinnostUpdate cmd);
        Task Remove(CommandCinnostRemove cmd);       
        Task LastEventCheck(Guid eventId, Guid entityId);
        Task ReplayEvents(List<string> msgstream,Guid? entityId);
        Task RequestEvents(Guid? entityId);






    }
}
