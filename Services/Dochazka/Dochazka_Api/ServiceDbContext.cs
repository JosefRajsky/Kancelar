
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Dochazka_Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Dochazka_Api
{
    public class ServiceDbContext : DbContext
    {
         public ServiceDbContext(DbContextOptions options) : base(options) {
            this.Database.EnsureCreated();
        }  
       
        public  DbSet<Dochazka> Dochazka { get; set; }


     



    }




}
