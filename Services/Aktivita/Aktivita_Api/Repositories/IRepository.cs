using CommandHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aktivita_Api.Repositories
{
    public interface IRepository
    {
        Task<List<Aktivita>> GetList();
        Task<Aktivita> Get(Guid id);
        Task Add(CommandAktivitaCreate cmd);
        Task Update(CommandAktivitaUpdate cmd);
        Task Remove(CommandAktivitaRemove cmd);       
        Task LastEventCheck(Guid eventId, Guid entityId);
        Task ReplayEvents(List<string> msgstream,Guid? entityId);
        Task RequestEvents(Guid? entityId);






    }
}
