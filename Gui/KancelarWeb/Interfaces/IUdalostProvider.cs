using KancelarWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.Interfaces
{
    public interface IUdalostProvider
    {
        public IEnumerable<UdalostModel> GetList();
        public UdalostModel Get(int id);

        public void Add(UdalostModel udalost);

        public void Remove(int id);
    }
}
