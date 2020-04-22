


using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Udalost_Api.Entities;

namespace Udalost_Api
{
    public class ServiceDbContext : DbContext
    {
         public ServiceDbContext(DbContextOptions options) : base(options) {
            this.Database.EnsureCreated();
            Database.Migrate();
        }  
       
        public  DbSet<Udalost> Udalosti { get; set; }
    }




}
