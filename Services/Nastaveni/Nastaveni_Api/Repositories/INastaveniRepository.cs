
using EventLibrary;
using Nastaveni_Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nastaveni_Api.Repositories
{
    public interface INastaveniRepository
    {
        Task<List<Nastaveni>> GetList();
        Task<Nastaveni> Get(string id);

        Task Add(CommandNastaveniCreate cmd);

        Task Update(CommandNastaveniUpdate cmd);

        Task Remove(CommandNastaveniRemove cmd);
         

    }
}
