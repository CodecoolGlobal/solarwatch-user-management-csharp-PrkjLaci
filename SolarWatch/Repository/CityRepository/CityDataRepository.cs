using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;
using SolarWatch.Models;

namespace SolarWatch.Repository.CityRepository;

public class CityDataRepository : ICityDataRepository
{
    private readonly ILogger<CityDataRepository> _logger;
    private readonly IConfiguration _configuration;
    
    public CityDataRepository(ILogger<CityDataRepository> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }
    
    public async Task<City?> GetCityData(string city)
    {
        await using var dbContext = new SolarWatchContext(_configuration);
        return await dbContext.CityData.FirstOrDefaultAsync(c => c.CityName == city);
    }

    public async Task SaveCityData(City city)
    {
        await using var dbContext = new SolarWatchContext(_configuration);
        var cityDataEntity = await dbContext.CityData.FirstOrDefaultAsync(c => c.CityName == city.CityName);

        if (cityDataEntity is null)
        {
            cityDataEntity = new City
            {
                CityName = city.CityName,
                Latitude = city.Latitude,
                Longitude = city.Longitude,
                State = city.State,
                Country = city.Country
            };
            await dbContext.CityData.AddAsync(cityDataEntity);
        }
        else
        {
            _logger.LogInformation($"City: {city.CityName} already exists in the database.");
        }
        await dbContext.SaveChangesAsync();
    }
}