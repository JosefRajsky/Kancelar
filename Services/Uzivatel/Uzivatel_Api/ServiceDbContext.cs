using Microsoft.EntityFrameworkCore;

namespace Uzivatel_Api
{
    public class ServiceDbContext : DbContext
    {
         public ServiceDbContext(DbContextOptions options) : base(options) {
         this.Database.EnsureCreated();
         this.Database.Migrate();
        }  
        public  DbSet<Uzivatel> Uzivatele { get; set; }
    }
}
