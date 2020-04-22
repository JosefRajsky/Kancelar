using Microsoft.EntityFrameworkCore;
namespace Nastaveni_Api
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions options) : base(options)
        {
            this.Database.EnsureCreated();
            Database.Migrate();
        }
        public DbSet<Pravidlo> Nastaveni { get; set; }
    }
}
