using KancelarWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.Interfaces
{
    public interface IUdalostReader
    {
        public IEnumerable<UdalostModel> GetList();
        public UdalostModel Get(int id);

    }
}
