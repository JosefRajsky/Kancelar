using Microsoft.EntityFrameworkCore;
using Nastaveni_Api.Entities;

namespace Nastaveni_Api
{
    public class NastaveniDbContext : DbContext
    {
         public NastaveniDbContext(DbContextOptions options) : base(options) {
            this.Database.EnsureCreated();
        }  
       
        public  DbSet<Nastaveni> NastaveniDochazky { get; set; }


     



    }




}
