using CommandHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template.Repositories
{
    public interface IRepository
    {
        Task<List<Temp>> GetList();
        Task<Temp> Get(Guid id);
        Task Add(CommandTempCreate cmd);
        Task Update(CommandTempUpdate cmd);
        Task Remove(CommandTempRemove cmd);       
        Task LastEventCheck(Guid eventId, Guid entityId);
        Task ReplayEvents(List<string> msgstream,Guid? entityId);
        Task RequestEvents(Guid? entityId);






    }
}
