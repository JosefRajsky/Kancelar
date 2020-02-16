using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Udalost_Service;

namespace Dochazka_Service.Repositories
{
  public class UdalostFactory : IDesignTimeDbContextFactory<UdalostDbContext>
{
        private string _connectionString;
        public UdalostFactory(string connectionString)
        {
            _connectionString = connectionString;
        }
        public UdalostDbContext CreateDbContext()
    {
        return CreateDbContext(null);
    } 
    public UdalostDbContext CreateDbContext(string[] args)
    {                
        var builder = new DbContextOptionsBuilder<UdalostDbContext>();
        builder.UseSqlServer(_connectionString); 
        return new UdalostDbContext(builder.Options);
    }
 
    
}
}
