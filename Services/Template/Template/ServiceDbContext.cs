using Microsoft.EntityFrameworkCore;
namespace Template
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions options) : base(options)
        {
            this.Database.EnsureCreated();
            Database.Migrate();
        }
        public DbSet<Temp> Temps { get; set; }
    }
}
