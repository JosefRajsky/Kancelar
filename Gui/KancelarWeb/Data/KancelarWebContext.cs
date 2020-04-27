using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KancelarWeb.ViewModels;
using Microsoft.EntityFrameworkCore;


namespace KancelarWeb.Data
{
    //Description: Dummy Context pro generovani views.
    public class KancelarWebContext : DbContext
    {
        public KancelarWebContext (DbContextOptions<KancelarWebContext> options)
            : base(options)
        {
        }

        public DbSet<Aktivita> Aktivita { get; set; }

        public DbSet<Kalendar> Kalendar { get; set; }

        public DbSet<Uzivatel> Uzivatel { get; set; }
    }
}
