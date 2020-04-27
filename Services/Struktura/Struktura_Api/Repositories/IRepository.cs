using CommandHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Struktura_Api.Repositories
{
    public interface IRepository
    {
        Task<List<Struktura>> GetList();
        Task<Struktura> Get(Guid id);
        //Task Add(CommandStrukturaCreate cmd);
        //Task Update(CommandStrukturaUpdate cmd);
        //Task Remove(CommandStrukturaRemove cmd);       
        Task LastEventCheck(Guid eventId, Guid entityId);
        Task ReplayEvents(List<string> msgstream,Guid? entityId);
        Task RequestEvents(Guid? entityId);
        Task CreateBySoucast(EventSoucastCreated evt);
        Task UpdateBySoucast(EventSoucastUpdated evt);
        Task DeleteBySoucast(EventSoucastRemoved evt);

        Task CreateByUzivatel(EventUzivatelCreated evt);
        Task UpdateByUzivatel(EventUzivatelUpdated evt);
        Task DeleteByUzivatel(EventUzivatelRemoved evt);





    }
}
