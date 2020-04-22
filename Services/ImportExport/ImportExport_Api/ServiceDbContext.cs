

using Microsoft.EntityFrameworkCore;

namespace ImportExport_Api
{
    public class ServiceDbContext : DbContext
    {
         public ServiceDbContext(DbContextOptions options) : base(options) {
         this.Database.EnsureCreated();
        }  
       
        public  DbSet<ImportExport> ImportExports { get; set; }
    }




}
