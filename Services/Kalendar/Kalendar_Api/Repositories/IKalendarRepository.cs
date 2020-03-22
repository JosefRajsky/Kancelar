

using EventLibrary;
using Kalendar_Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kalendar_Api.Repositories
{
    public interface IKalendarRepository
    {
        Task<IEnumerable<KalendarModel>> GetList();
        Task<Kalendar> Get(string id);
        DateTime GetLast();
        Task Add(CommandKalendarCreate cmd, bool publish);

        Task Update(CommandKalendarUpdate cmd, bool publish);
        Task UpdateByUdalost(EventUdalostCreated evt);
        Task AddByUzivatel(EventUzivatelCreated evt);
       
        Task UpdateByUzivatel(EventUzivatelUpdated evt);        
        Task DeleteByUzivatel(EventUzivatelDeleted evt);
    }
}
