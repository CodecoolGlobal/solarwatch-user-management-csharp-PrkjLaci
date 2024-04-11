using Microsoft.EntityFrameworkCore;
using SolarWatch.Models;

namespace SolarWatch.Data;

public class SolarWatchContext : DbContext
{
    public DbSet<City> CityData { get; set; }
    public DbSet<SunsetSunriseTime> SunsetSunriseTime { get; set; }

    public SolarWatchContext(DbContextOptions<SolarWatchContext> options) : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<City>()
            .HasIndex(u => u.Id)
            .IsUnique();
        
        modelBuilder.Entity<SunsetSunriseTime>()
            .HasOne(s => s.City)
            .WithMany(c => c.SunsetSunriseTime)
            .HasForeignKey(s => s.CityId);
        
        modelBuilder.Entity<SunsetSunriseTime>()
            .Property(p => p.Id)
            .ValueGeneratedOnAdd();
    }
    
}