using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventStore
{ 
        public class EventStoreDbContextFactory : IDesignTimeDbContextFactory<EventStoreDbContext>
        {
            private string _connectionString;
            public EventStoreDbContextFactory(string connectionString)
            {
                _connectionString = connectionString;
            }
        public EventStoreDbContext CreateDbContext()
            {
                return CreateDbContext(null);
            }
            public EventStoreDbContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<EventStoreDbContext>();
                builder.UseSqlServer(_connectionString);
                return new EventStoreDbContext(builder.Options);
            }
        }    
}

