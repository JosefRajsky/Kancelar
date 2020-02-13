﻿using KancelarWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KancelarWeb.Interfaces
{
    public interface IUdalostProvider
    {
        IEnumerable<UdalostModel> GetList();
        UdalostModel Get(int id);

        bool Add(UdalostModel udalost);

        bool Delete(int id);
    }
}
