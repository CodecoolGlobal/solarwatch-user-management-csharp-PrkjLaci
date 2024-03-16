using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;
using SolarWatch.Models;
using SolarWatch.Repository.CityRepository;

namespace SolarWatch.Repository.SunsetSunriseRepository;

public class SunsetSunriseRepository : ISunsetSunriseRepository
{
    private readonly ILogger<SunsetSunriseRepository> _logger;
    private readonly IConfiguration _configuration;
    private readonly ICityDataRepository _cityDataRepository;
    private readonly SolarWatchContext _dbContext;
    
    public SunsetSunriseRepository(ILogger<SunsetSunriseRepository> logger, IConfiguration configuration, ICityDataRepository cityDataRepository, SolarWatchContext dbContext)
    {
        _logger = logger;
        _configuration = configuration;
        _cityDataRepository = cityDataRepository;
        _dbContext = dbContext;
    }
    
    public async Task<SunsetSunriseTime?> GetSunsetSunrise(string city, string date)
    {
        await using var dbContext = new SolarWatchContext(_configuration);
        return await dbContext.SunsetSunriseTime.FirstOrDefaultAsync(c => c.City != null && c.City.CityName == city && c.Date == date);
    }

    public async Task AddSunsetSunrise(SunsetSunriseTime sunsetSunrise, int cityId)
    {
        var city = await _dbContext.CityData.Include(city => city.SunsetSunriseTime).FirstOrDefaultAsync(c => c.Id == cityId);
        
        if (city is null)
        {
            throw new Exception("City not found.");
        }
        
        sunsetSunrise.CityId = city.Id;
        city.SunsetSunriseTime.Add(sunsetSunrise);
        await _dbContext.SunsetSunriseTime.AddAsync(sunsetSunrise);
        await _dbContext.SaveChangesAsync();
        
    }

    public async Task<SunsetSunriseTime> UpdateSunsetSunrise(SunsetSunriseTime sunsetSunrise, int cityId, int sunsetSunriseId)
    {
        var city = await _dbContext.CityData.Include(city => city.SunsetSunriseTime).FirstOrDefaultAsync(c => c.Id == cityId);
        
        if (city is null)
        {
            throw new Exception("City not found.");
        }
        
        var sunsetSunriseInDb = city.SunsetSunriseTime.FirstOrDefault(s => s.Id == sunsetSunriseId);
        
        if (sunsetSunriseInDb is null)
        {
            throw new Exception("SunsetSunrise not found.");
        }
        
        sunsetSunriseInDb.Date = sunsetSunrise.Date;
        sunsetSunriseInDb.Sunrise = sunsetSunrise.Sunrise;
        sunsetSunriseInDb.Sunset = sunsetSunrise.Sunset;
        
        await _dbContext.SaveChangesAsync();

        return sunsetSunriseInDb;
    }

    public async Task DeleteSunsetSunrise(int id)
    {
        var sunsetSunriseToDelete = await _dbContext.SunsetSunriseTime.FirstOrDefaultAsync(s => s.Id == id);
        
        if (sunsetSunriseToDelete is null)
        {
            throw new Exception("SunsetSunrise not found.");
        }
        
        _dbContext.SunsetSunriseTime.Remove(sunsetSunriseToDelete);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SaveSunsetSunrise(City? city, SunsetSunriseTime sunsetSunrise)
    {
        await using var dbContext = new SolarWatchContext(_configuration);
        var cityEntity = await dbContext.CityData.FirstOrDefaultAsync(c => city != null && c.CityName == city.CityName);

        if (cityEntity is null)
        { 
            await dbContext.CityData.AddAsync(city);
            await dbContext.SaveChangesAsync();
            cityEntity = city;
        }
        else
        {
            _logger.LogInformation($"City: {city.CityName} already exists in the database.");
        }
        
        sunsetSunrise.City = cityEntity;
        sunsetSunrise.CityId = cityEntity.Id;
        await dbContext.SunsetSunriseTime.AddAsync(sunsetSunrise);

        
        await dbContext.SaveChangesAsync();
    }


}