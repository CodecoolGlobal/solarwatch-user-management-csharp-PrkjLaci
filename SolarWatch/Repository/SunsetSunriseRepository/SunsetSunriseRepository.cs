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
        return await _dbContext.SunsetSunriseTime.FirstOrDefaultAsync(c => c.City != null && c.City.CityName == city && c.Date == date);
    }
    public async Task<List<SunsetSunriseTime>> GetAllSunsetSunrise()
    {
        return await _dbContext.SunsetSunriseTime.ToListAsync();
    }

    public async Task AddSunsetSunrise(SunsetSunriseTime sunsetSunrise)
    {
        var city = await _dbContext.CityData.Include(city => city.SunsetSunriseTime).FirstOrDefaultAsync(c => c.Id == sunsetSunrise.CityId);
        
        if (city is null)
        {
            throw new Exception("City not found.");
        }
        
        city.SunsetSunriseTime.Add(sunsetSunrise);
        if (!_dbContext.SunsetSunriseTime.Any(s =>
                s.Date == sunsetSunrise.Date && s.Sunset == sunsetSunrise.Sunset && s.Sunrise == sunsetSunrise.Sunrise))
        {
            await _dbContext.SunsetSunriseTime.AddAsync(sunsetSunrise);
        }
        else
        {
            throw new Exception("Sunset sunrise already in the database.");
        }
        await _dbContext.SaveChangesAsync();
        
    }

    public async Task<SunsetSunriseTime> UpdateSunsetSunrise(SunsetSunriseTime sunsetSunrise)
    {
        var city = await _dbContext.CityData.Include(city => city.SunsetSunriseTime).FirstOrDefaultAsync(c => c.Id == sunsetSunrise.CityId);
        
        if (city is null)
        {
            throw new Exception("City not found.");
        }
        
        var sunsetSunriseToUpdate = city.SunsetSunriseTime.FirstOrDefault(s => s.Id == sunsetSunrise.Id);
        
        if (sunsetSunriseToUpdate is null)
        {
            throw new Exception("SunsetSunrise not found.");
        }

        _dbContext.SunsetSunriseTime.Entry(sunsetSunriseToUpdate).CurrentValues.SetValues(sunsetSunrise);
        
        await _dbContext.SaveChangesAsync();
        
        return sunsetSunriseToUpdate;
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
        var cityEntity = await _dbContext.CityData.FirstOrDefaultAsync(c => city != null && c.CityName == city.CityName);

        if (cityEntity is null)
        { 
            await _dbContext.CityData.AddAsync(city);
            await _dbContext.SaveChangesAsync();
            cityEntity = city;
        }
        else
        {
            _logger.LogInformation($"City: {city.CityName} already exists in the database.");
        }
        
        sunsetSunrise.City = cityEntity;
        sunsetSunrise.CityId = cityEntity.Id;
        await _dbContext.SunsetSunriseTime.AddAsync(sunsetSunrise);

        
        await _dbContext.SaveChangesAsync();
    }


}