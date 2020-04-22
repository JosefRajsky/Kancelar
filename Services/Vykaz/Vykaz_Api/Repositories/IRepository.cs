using CommandHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vykaz_Api.Repositories
{
    public interface IRepository
    {
        Task<List<Vykaz>> GetList();
        Task<Vykaz> Get(Guid id);
        Task Add(CommandVykazCreate cmd);
        Task Update(CommandVykazUpdate cmd);
        Task Remove(CommandVykazRemove cmd);       
        Task LastEventCheck(Guid eventId, Guid entityId);
        Task ReplayEvents(List<string> msgstream,Guid? entityId);
        Task RequestEvents(Guid? entityId);






    }
}
