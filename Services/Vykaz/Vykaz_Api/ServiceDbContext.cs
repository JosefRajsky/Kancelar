using Microsoft.EntityFrameworkCore;
namespace Vykaz_Api
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions options) : base(options)
        {
            this.Database.EnsureCreated();
            Database.Migrate();
        }
        public DbSet<Vykaz> Vykazy { get; set; }
    }
}
