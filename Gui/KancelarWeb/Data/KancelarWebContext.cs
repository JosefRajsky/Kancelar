using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KancelarWeb.ViewModels;

namespace KancelarWeb.Data
{
    //Description: Dummy Context pro generovani views.
    public class KancelarWebContext : DbContext
    {
        public KancelarWebContext (DbContextOptions<KancelarWebContext> options)
            : base(options)
        {
        }

        public DbSet<KancelarWeb.ViewModels.UdalostModel> UdalostModel { get; set; }

        public DbSet<KancelarWeb.Kalendar> Kalendar { get; set; }

        public DbSet<KancelarWeb.ViewModels.Uzivatel> Uzivatel { get; set; }
    }
}
