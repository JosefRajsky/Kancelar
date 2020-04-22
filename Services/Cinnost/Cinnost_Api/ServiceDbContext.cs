using Microsoft.EntityFrameworkCore;
namespace Cinnost_Api
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions options) : base(options)
        {
            this.Database.EnsureCreated();
        }
        public DbSet<Cinnost> Cinnosti { get; set; }
    }
}
