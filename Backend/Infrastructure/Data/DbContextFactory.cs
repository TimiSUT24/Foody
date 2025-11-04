using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public sealed class DbContextFactory : IDesignTimeDbContextFactory<DbContext>
    {
        public DbContext CreateDbContext(string[] args)
        {
            // Determine environment (Development / Production / etc.)
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            // Locate the API project's path (startup project)
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../Api");

            // Build configuration from API project’s appsettings
            var configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // Get connection string
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }
               
            // Configure options
            var optionsBuilder = new DbContextOptionsBuilder<DbContext>()
                .UseNpgsql(connectionString, options =>
                {
                    // Ensure migrations stay in the Infrastructure project
                    options.MigrationsAssembly(typeof(DbContext).Assembly.FullName);
                });

            return new DbContext(optionsBuilder.Options);
        }
    }
}
