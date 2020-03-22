

using EventLibrary;
using EventStore_Api;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventStore_Api.Repositories
{
    public interface IEventStoreRepository
    {
        Task AddMessageAsync(string msg);
        Task<List<StoreMessage>> GetList();
        Task<StoreMessage> Get(string id);

        Task<List<StoreMessage>> GetListByDate(DateTime datum);
        void ServiceHeal(string msg);
    }
}
