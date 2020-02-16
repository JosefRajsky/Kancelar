using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Udalost_Service;

namespace Dochazka_Service.Repositories
{
  public class DochazkaDbContextFactory : IDesignTimeDbContextFactory<DochazkaDbContext>
{
        private string _connectionString;

        public DochazkaDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DochazkaDbContext CreateDbContext()
    {
        return CreateDbContext(null);
    }
 
    public DochazkaDbContext CreateDbContext(string[] args)
    {
                
        var builder = new DbContextOptionsBuilder<DochazkaDbContext>();
        builder.UseSqlServer(_connectionString);
 
        return new DochazkaDbContext(builder.Options);
    }
 
    
}
}
