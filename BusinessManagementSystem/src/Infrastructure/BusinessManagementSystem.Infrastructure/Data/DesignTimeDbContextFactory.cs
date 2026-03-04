using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;

namespace BusinessManagementSystem.Infrastructure.Data
{
    /// <summary>
    /// Design-time factory used by EF Core tools to create the <see cref="ApplicationDbContext" />.
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            // You can override the connection string by providing an environment variable
            // named "BMS_CONNECTION_STRING" or pass it via command line args.
            string? connectionString = null;

            if (args != null && args.Length > 0)
            {
                // assume first argument is connection string for convenience
                connectionString = args[0];
            }

            connectionString ??= Environment.GetEnvironmentVariable("BMS_CONNECTION_STRING");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                // fallback to a default local PostgreSQL connection
                connectionString = "Host=localhost;Database=BusinessManagementDb;Username=postgres;Password=postgres";
            }

            // choose provider based on connection string contents
            if (connectionString.Contains("Data Source", StringComparison.OrdinalIgnoreCase))
            {
                // use SQLite when a file-based connection is provided
                optionsBuilder.UseSqlite(connectionString);
            }
            else
            {
                // default to PostgreSQL
                optionsBuilder.UseNpgsql(connectionString);
            }

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
