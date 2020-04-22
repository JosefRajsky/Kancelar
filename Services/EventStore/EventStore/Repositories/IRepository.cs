


using EventStore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventStore.Repositories
{
    public interface IRepository
    {
        Task AddMessageAsync(string msg);
        Task<StoreMessage> Get(string id);    
        Task ProvideHealingStream(string msg);
    }
}
