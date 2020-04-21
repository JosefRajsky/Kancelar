using Microsoft.EntityFrameworkCore;


namespace Uzivatel_Api
{
    public class UzivatelDbContext : DbContext
    {
         public UzivatelDbContext(DbContextOptions options) : base(options) {
         this.Database.EnsureCreated();
         this.Database.Migrate();
        }  
        public  DbSet<Uzivatel> Uzivatele { get; set; }
    }
}
