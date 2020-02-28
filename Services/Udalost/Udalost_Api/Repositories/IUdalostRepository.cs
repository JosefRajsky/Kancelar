
using EventLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Udalost_Api.Entities;
using Udalost_Api.Models;

namespace Udalost_Api.Repositories
{
    public interface IUdalostRepository
    {
        Task<List<Udalost>> GetList();
        Task<Udalost> Get(int id);

        Task Add(CommandUdalostCreate cmd);

        Task Update(CommandUdalostUpdate cmd);

        Task Remove(CommandUdalostRemove cmd);
         

    }
}
