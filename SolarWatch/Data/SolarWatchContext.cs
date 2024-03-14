using Microsoft.EntityFrameworkCore;
using SolarWatch.Models;

namespace SolarWatch.Data;

public class SolarWatchContext : DbContext
{
    private readonly IConfiguration _configuration;
    public DbSet<City> CityData { get; set; }
    public DbSet<SunsetSunriseTime> SunsetSunriseTime { get; set; }

    public SolarWatchContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? connectionString = _configuration["DB_CONNECTION_STRING"];
        optionsBuilder.UseSqlServer(connectionString);
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>()
            .HasIndex(u => u.State)
            .IsUnique();
        
        modelBuilder.Entity<City>()
            .HasOne(c => c.SunsetSunriseTime)
            .WithOne(s => s.City)
            .HasForeignKey<SunsetSunriseTime>(s => s.Id);
    }
    
}