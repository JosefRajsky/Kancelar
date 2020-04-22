using Microsoft.EntityFrameworkCore;
namespace Struktura_Api
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions options) : base(options)
        {
            this.Database.EnsureCreated();
            Database.Migrate();
        }
        public DbSet<Struktura> Struktury { get; set; }
    }
}
