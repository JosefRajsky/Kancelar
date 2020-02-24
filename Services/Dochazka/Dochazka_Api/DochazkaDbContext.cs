


using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Dochazka_Api.Entities;
using DochazkaLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace Dochazka_Api
{
    public class DochazkaDbContext : DbContext
    {
         public DochazkaDbContext(DbContextOptions options) : base(options) {
            this.Database.EnsureCreated();
        }  
       
        public  DbSet<Dochazka> Dochazka { get; set; }


     



    }




}
