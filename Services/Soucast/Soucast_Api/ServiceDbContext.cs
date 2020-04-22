using Microsoft.EntityFrameworkCore;
namespace Soucast_Api
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions options) : base(options)
        {
            this.Database.EnsureCreated();
            Database.Migrate();
        }
        public DbSet<Soucast> Soucasti { get; set; }
    }
}
