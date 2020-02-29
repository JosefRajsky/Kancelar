
using EventLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventStore_Api.Repositories
{
    public interface IEventStoreRepository
    {
        Task AddMessageAsync(string msg);



    }
}
