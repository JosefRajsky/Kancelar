
using CommandHandler;
using Dochazka_Api.Models;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dochazka_Api.Repositories
{
    public interface IRepository
    {
        Task<List<Dochazka>> GetList();
        Task<Dochazka> Get(Guid id);
        Task Add(CommandDochazkaCreate cmd);
        Task Update(CommandDochazkaUpdate cmd);
        Task Remove(CommandDochazkaRemove cmd);
        Task LastEventCheck(Guid eventId, Guid entityId);
        Task ReplayEvents(List<string> msgstream, Guid? entityId);
        Task RequestEvents(Guid? entityId);


    }
}
