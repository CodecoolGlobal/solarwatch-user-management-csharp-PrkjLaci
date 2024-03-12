using SolarWatch.Data;

namespace SolarWatch.Repository.CityRepository;

public class CityDataRepository : ICityDataRepository
{
    private readonly ILogger<CityDataRepository> _logger;
    
    public CityDataRepository(ILogger<CityDataRepository> logger)
    {
        _logger = logger;
    }
    
    public Models.City? GetCityData(string city)
    {
        using var dbContext = new SolarWatchContext();
        return dbContext.CityData.FirstOrDefault(c => c.CityName == city);
    }

    public void SaveCityData(Models.City city)
    {
        using var dbContext = new SolarWatchContext();
        var cityDataEntity = dbContext.CityData.FirstOrDefault(c => c.CityName == city.CityName);

        if (cityDataEntity is null)
        {
            cityDataEntity = new Models.City
            {
                CityName = city.CityName,
                Latitude = city.Latitude,
                Longitude = city.Longitude,
                State = city.State,
                Country = city.Country
            };
            dbContext.CityData.Add(cityDataEntity);
        }
        else
        {
            _logger.LogInformation($"City: {city.CityName} already exists in the database.");
        }
        dbContext.SaveChanges();
    }
}