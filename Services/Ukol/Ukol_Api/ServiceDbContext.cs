using Microsoft.EntityFrameworkCore;
namespace Ukol_Api
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions options) : base(options)
        {
            this.Database.EnsureCreated();
            Database.Migrate();
        }
        public DbSet<Ukol> Ukoly { get; set; }
    }
}
