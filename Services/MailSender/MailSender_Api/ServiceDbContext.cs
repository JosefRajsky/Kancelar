using Microsoft.EntityFrameworkCore;
namespace MailSender_Api
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions options) : base(options)
        {
            this.Database.EnsureCreated();
            Database.Migrate();
        }
        public DbSet<Mail> Mails { get; set; }
    }
}
