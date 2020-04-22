
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pritomnost_Api.Models;

namespace Pritomnost_Api
{
    public class ServiceDbContext : DbContext
    {
         public ServiceDbContext(DbContextOptions options) : base(options) {
            this.Database.EnsureCreated();
            this.Database.Migrate();
        }  
       
        public  DbSet<Pritomnost> Pritomnosti { get; set; }


     



    }




}
