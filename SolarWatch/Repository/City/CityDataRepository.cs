using SolarWatch.Data;
using SolarWatch.Models;

namespace SolarWatch.Repository.City;

public class CityDataRepository : ICityDataRepository
{
    private readonly ILogger<CityDataRepository> _logger;
    
    public CityDataRepository(ILogger<CityDataRepository> logger)
    {
        _logger = logger;
    }
    
    public CityData? GetCityData(string city)
    {
        using var dbContext = new SolarWatchContext();
        return dbContext.CityData.FirstOrDefault(c => c.CityName == city);
    }

    public void SaveCityData(CityData cityData)
    {
        using var dbContext = new SolarWatchContext();
        var cityDataEntity = dbContext.CityData.FirstOrDefault(c => c.CityName == cityData.CityName);

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
            dbContext.CityData.Add(cityDataEntity);
        }
        else
        {
            _logger.LogInformation($"City: {cityData.CityName} already exists in the database.");
        }
        dbContext.SaveChanges();
    }
}