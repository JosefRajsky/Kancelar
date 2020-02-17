using KancelarWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.Interfaces
{
    public interface IUdalostPublisher        
    {
      
            Task Add(UdalostModel udalost);
            Task Update(UdalostModel udalost);
            Task Remove(int id);
    }
}
