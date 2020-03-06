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

        public DbSet<Message> Messages { get; set; }
    }

}

