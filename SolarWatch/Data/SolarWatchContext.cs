using Microsoft.EntityFrameworkCore;
using SolarWatch.Models;

namespace SolarWatch.Data;

public class SolarWatchContext : DbContext
{
    public DbSet<CityData> CityData { get; set; }
    public DbSet<SunsetSunriseTime> SunsetSunriseTime { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=SolarWatch;User Id=sa;Password=Codecool12__;Encrypt=false;"
        );
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CityData>()
            .HasOne(c => c.SunsetSunriseTime)
            .WithOne()
            .HasForeignKey<CityData>(c => c.SunsetSunriseTimeId);
    }
    
}