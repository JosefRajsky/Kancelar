


using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Web_Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Web_Api
{
    public class DochazkaDbContext : DbContext
    {
         public DochazkaDbContext(DbContextOptions options) : base(options) {
            this.Database.EnsureCreated();
        }  
       
        public  DbSet<Dochazka> Dochazka { get; set; }


     



    }




}
