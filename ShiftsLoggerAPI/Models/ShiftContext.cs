using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using Microsoft.Identity.Client;
using System.Data.Common;

namespace ShiftsLoggerAPI.Models;

public class ShiftContext : DbContext
{
    public string connectionString;
    public DbSet<Shift> Shifts { get; set; } = null!;

    public ShiftContext(DbContextOptions<ShiftContext> options) : base(options)
    {
        IConfigurationRoot configuration =
            new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        connectionString = configuration.GetConnectionString("DbContext") ?? "";
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;

        IConfigurationRoot configuration =
                new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
        optionsBuilder.UseSqlServer(connectionString);
    }
}
