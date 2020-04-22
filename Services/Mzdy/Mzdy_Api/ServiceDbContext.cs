using Microsoft.EntityFrameworkCore;
namespace Mzdy_Api
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions options) : base(options)
        {
            this.Database.EnsureCreated();
            Database.Migrate();
        }
        public DbSet<Mzda> Mzdy { get; set; }
    }
}
