
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Udalost_Service.Entities;

namespace Udalost_Service
{
    public class UdalostDbContext : DbContext
    {
         public UdalostDbContext(DbContextOptions options) : base(options) {
            this.Database.EnsureCreated();
        }  
       
        public  DbSet<Udalost> Udalosti { get; set; }


     



    }




}
