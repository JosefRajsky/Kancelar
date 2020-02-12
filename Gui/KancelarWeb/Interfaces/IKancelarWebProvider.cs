using KancelarWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.Interfaces
{
    public interface IKancelarWebProvider
    {
        IEnumerable<Udalost> UdalostList();
        Udalost UdalostGet(int id);

        bool UdalostAdd(Udalost udalost);

        bool UdalostRemove(int id);
    }
}
