

using CommandHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Import.Repositories
{
    public interface IRepository
    {

        Task ImportUzivatel(string uzivatele);  
    }
}
