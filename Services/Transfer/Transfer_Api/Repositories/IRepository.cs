using CommandHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfer_Api.Repositories
{
    public interface IRepository
    {
        Task<List<Transfer>> GetList();
        Task<Transfer> Get(Guid id);
        Task Add(CommandTransferCreate cmd);
        Task Update(CommandTransferUpdate cmd);
        Task Remove(CommandTransferRemove cmd);       
        Task LastEventCheck(Guid eventId, Guid entityId);
        Task ReplayEvents(List<string> msgstream,Guid? entityId);
        Task RequestEvents(Guid? entityId);






    }
}
