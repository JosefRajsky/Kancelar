using Microsoft.EntityFrameworkCore;
namespace Transfer_Api
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions options) : base(options)
        {
            this.Database.EnsureCreated();
            Database.Migrate();
        }
        public DbSet<Transfer> Transfers { get; set; }
    }
}
