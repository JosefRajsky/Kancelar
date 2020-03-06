
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Kalendar_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Kalendar_Api
{
    public class KalendarDbContext : DbContext
    {
         public KalendarDbContext(DbContextOptions options) : base(options) {
            this.Database.EnsureCreated();
        }  
       
        public  DbSet<Kalendar> Kalendare { get; set; }


     



    }




}
