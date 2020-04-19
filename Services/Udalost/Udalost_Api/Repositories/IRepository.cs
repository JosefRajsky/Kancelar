

using CommandHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Udalost_Api.Entities;

namespace Udalost_Api.Repositories
{
    public interface IRepository
    {
        Task LastEventCheck(Guid eventId, Guid entityId);
        Task ReplayEvents(List<string> msgstream, Guid? entityId);
        Task RequestEvents(Guid? entityId);
        Task<List<Udalost>> GetList();
        Task<Udalost> Get(Guid udalostId);

        Task Add(CommandUdalostCreate cmd);

        Task Update(CommandUdalostUpdate cmd);

        Task Remove(CommandUdalostRemove cmd);


         

    }
}
