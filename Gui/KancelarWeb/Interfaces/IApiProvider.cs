using KancelarWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.Interfaces
{
    public interface IApiProvider
    {
        Task<T> List<T>(string apibase);
        Task<T> Get<T>(int id, string apibase);
        Task Add<T>(T model, string apibase);
        Task Update<T>(T model,string apibase);
        Task Remove(int id, string apibase);
    }
}
