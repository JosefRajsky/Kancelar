using Microsoft.EntityFrameworkCore;

namespace ImportExport_Api
{
    public class ImportExportDbContext : DbContext
    {
         public ImportExportDbContext(DbContextOptions options) : base(options) {
         this.Database.EnsureCreated();
         this.Database.Migrate();
        }  
       
        public  DbSet<ImportExport> ImportExports { get; set; }
    }




}
