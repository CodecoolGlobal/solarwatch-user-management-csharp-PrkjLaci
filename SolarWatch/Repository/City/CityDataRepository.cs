using SolarWatch.Data;
using SolarWatch.Models;

namespace SolarWatch.Repository.City;

public class CityDataRepository : ICityDataRepository
{
    private readonly ILogger<CityDataRepository> _logger;
    private readonly SolarWatchContext _dbContext;
    
    public CityDataRepository(ILogger<CityDataRepository> logger, SolarWatchContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public CityData? GetCityData(string city)
    {
        return _dbContext.CityData.FirstOrDefault(c => c.CityName == city);
    }

    public void SaveCityData(CityData cityData)
    {
        var cityDataEntity = _dbContext.CityData.FirstOrDefault(c => c.CityName == cityData.CityName);

        if (cityDataEntity is null)
        {
            cityDataEntity = new CityData
            {
                CityName = cityData.CityName,
                Latitude = cityData.Latitude,
                Longitude = cityData.Longitude,
                State = cityData.State,
                Country = cityData.Country
            };
            _dbContext.CityData.Add(cityDataEntity);
        }
        else
        {
            _logger.LogInformation($"City: {cityData.CityName} already exists in the database.");
        }
        _dbContext.SaveChanges();
    }
}