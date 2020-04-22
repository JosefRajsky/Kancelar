using Microsoft.EntityFrameworkCore;
namespace Opravneni_Api
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions options) : base(options)
        {
            this.Database.EnsureCreated();
            Database.Migrate();
        }
        public DbSet<Pravo> Opravneni { get; set; }
    }
}
