using CommandHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opravneni_Api.Repositories
{
    public interface IRepository
    {
        Task<List<Pravo>> GetList();
        Task<Pravo> Get(Guid id);
        Task Add(CommandOpravneniCreate cmd);
        Task Update(CommandOpravneniUpdate cmd);
        Task Remove(CommandOpravneniRemove cmd);       
        Task LastEventCheck(Guid eventId, Guid entityId);
        Task ReplayEvents(List<string> msgstream,Guid? entityId);
        Task RequestEvents(Guid? entityId);






    }
}
