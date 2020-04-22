using CommandHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nastaveni_Api.Repositories
{
    public interface IRepository
    {
        Task<List<Pravidlo>> GetList();
        Task<Pravidlo> Get(Guid id);
        Task Add(CommandNastaveniCreate cmd);
        Task Update(CommandNastaveniUpdate cmd);
        Task Remove(CommandNastaveniRemove cmd);       
        Task LastEventCheck(Guid eventId, Guid entityId);
        Task ReplayEvents(List<string> msgstream,Guid? entityId);
        Task RequestEvents(Guid? entityId);






    }
}
