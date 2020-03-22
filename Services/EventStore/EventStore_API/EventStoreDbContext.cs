using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;

namespace EventStore_Api
{

    public class EventStoreDbContext : DbContext
    {
        public EventStoreDbContext(DbContextOptions options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<StoreMessage> Messages { get; set; }
    }

}

