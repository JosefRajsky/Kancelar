

using CommandHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transfer_Api.Repositories
{
    public interface IRepository
    {
        Task LastEventCheck(Guid eventId, Guid entityId);
        Task ReplayEvents(List<string> msgstream, Guid? entityId);
        Task RequestEvents(Guid? entityId);
        Task Add(string uzivatele);
       






    }
}
