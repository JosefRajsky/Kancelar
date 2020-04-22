
using Microsoft.EntityFrameworkCore;

namespace Aktivita_Api
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions options) : base(options)
        {
            this.Database.EnsureCreated();
        }
        public DbSet<Aktivita> Aktivity { get; set; }
    }
}
