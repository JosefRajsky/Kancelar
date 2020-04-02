using Microsoft.EntityFrameworkCore;
namespace Template
{
    public class TemplateDbContext : DbContext
    {
         public TemplateDbContext(DbContextOptions options) : base(options) {
         this.Database.EnsureCreated();
        }  
        public  DbSet<Temp> Temps { get; set; }
    }
}
