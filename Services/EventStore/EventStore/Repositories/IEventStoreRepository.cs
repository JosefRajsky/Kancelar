


using EventStore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventStore.Repositories
{
    public interface IEventStoreRepository
    {
        Task AddMessageAsync(string msg);
        Task<StoreMessage> Get(string id);    
        Task ProvideHealingStream(string msg);
    }
}
