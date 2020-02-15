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
    private static string _connectionString;
 
    public DochazkaDbContext CreateDbContext()
    {
        return CreateDbContext(null);
    }
 
    public DochazkaDbContext CreateDbContext(string[] args)
    {
        if (string.IsNullOrEmpty(_connectionString))
        {
            LoadConnectionString();
        }
        _connectionString = "Server=sqlServer;Initial Catalog=Kancelar.Service.Dochazka;User=sa;Password=Password123;MultipleActiveResultSets=true;";
        var builder = new DbContextOptionsBuilder<DochazkaDbContext>();
        builder.UseSqlServer(_connectionString);
 
        return new DochazkaDbContext(builder.Options);
    }
 
    private static void LoadConnectionString()
    {
        var builder = new ConfigurationBuilder();
        builder.AddJsonFile("appsettings.json", optional: false);
 
        var configuration = builder.Build();
 
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
}
}
