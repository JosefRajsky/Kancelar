using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventStore
{ 
        public class EventStoreDbContextFactory : IDesignTimeDbContextFactory<ServiceDbContext>
        {
            private readonly string _connectionString;
            public EventStoreDbContextFactory(string connectionString)
            {
                _connectionString = connectionString;
            }
        public ServiceDbContext CreateDbContext()
            {
                return CreateDbContext(null);
            }
            public ServiceDbContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<ServiceDbContext>();
                builder.UseSqlServer(_connectionString);
                return new ServiceDbContext(builder.Options);
            }
        }    
}

