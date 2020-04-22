using CommandHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Soucast_Api.Repositories
{
    public interface IRepository
    {
        Task<List<Soucast>> GetList();
        Task<Soucast> Get(Guid id);
        Task Add(CommandSoucastCreate cmd);
        Task Update(CommandSoucastUpdate cmd);
        Task Remove(CommandSoucastRemove cmd);       
        Task LastEventCheck(Guid eventId, Guid entityId);
        Task ReplayEvents(List<string> msgstream,Guid? entityId);
        Task RequestEvents(Guid? entityId);






    }
}
