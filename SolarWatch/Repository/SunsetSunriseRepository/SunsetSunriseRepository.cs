﻿using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;
using SolarWatch.Models;

namespace SolarWatch.Repository.SunsetSunriseRepository;

public class SunsetSunriseRepository : ISunsetSunriseRepository
{
    private readonly ILogger<SunsetSunriseRepository> _logger;
    
    public SunsetSunriseRepository(ILogger<SunsetSunriseRepository> logger)
    {
        _logger = logger;
    }
    
    public async Task<SunsetSunriseTime?> GetSunsetSunrise(string city, string date)
    {
        await using var dbContext = new SolarWatchContext();
        return await dbContext.SunsetSunriseTime.FirstOrDefaultAsync(c => c.City != null && c.City.CityName == city && c.Date == date);
    }

    public async Task SaveSunsetSunrise(City? city, SunsetSunriseTime sunsetSunrise)
    {
        await using var dbContext = new SolarWatchContext();
        var cityEntity = await dbContext.CityData.FirstOrDefaultAsync(c => city != null && c.Id == city.Id);

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

        cityEntity.SunsetSunriseTimeId = sunsetSunrise.Id;
        
        await dbContext.SaveChangesAsync();
    }


}