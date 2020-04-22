using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;

namespace EventStore
{

    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<StoreMessage> Messages { get; set; }
    }

}

