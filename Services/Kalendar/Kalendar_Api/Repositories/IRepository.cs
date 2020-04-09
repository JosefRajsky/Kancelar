


using CommandHandler;
using Kalendar_Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kalendar_Api.Repositories
{
    public interface IRepository
    {
        Task<List<Kalendar>> GetList();
        Task<Kalendar> Get(Guid id);
        Task LastEventCheck(Guid eventId, Guid entityId);
   
        Task RequestEvents(Guid? entityId);
        Task CreateByUdalost(EventUdalostCreated evt);
        Task UpdateByUdalost(EventUdalostUpdated evt);
        Task DeleteByUdalost(EventUdalostRemoved evt);

        Task CreateByUzivatel(EventUzivatelCreated evt);       
        Task UpdateByUzivatel(EventUzivatelUpdated evt);        
        Task DeleteByUzivatel(EventUzivatelDeleted evt);
    }
}
