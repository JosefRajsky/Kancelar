

using EventLibrary;
using Kalendar_Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kalendar_Api.Repositories
{
    public interface IKalendarRepository
    {
        Task<IEnumerable<KalendarModel>> GetList();
        Task<Kalendar> Get(string id);

        Task Add(CommandKalendarCreate cmd);

        Task Update(CommandKalendarUpdate cmd);
        Task UpdateByUdalost(EventUdalostCreated evt);
        Task AddByUzivatel(EventUzivatelCreated evt);
    }
}
