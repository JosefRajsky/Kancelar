using CommandHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mzdy_Api.Repositories
{
    public interface IRepository
    {
        Task<List<Mzda>> GetList();
        Task<Mzda> Get(Guid id);
        Task Add(CommandMzdyCreate cmd);
        Task Update(CommandMzdyUpdate cmd);
        Task Remove(CommandMzdyRemove cmd);       
        Task LastEventCheck(Guid eventId, Guid entityId);
        Task ReplayEvents(List<string> msgstream,Guid? entityId);
        Task RequestEvents(Guid? entityId);






    }
}
