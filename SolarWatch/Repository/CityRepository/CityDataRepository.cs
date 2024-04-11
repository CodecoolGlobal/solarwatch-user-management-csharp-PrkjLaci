﻿using Microsoft.EntityFrameworkCore;
using SolarWatch.Data;
using SolarWatch.Models;

namespace SolarWatch.Repository.CityRepository;

public class CityDataRepository : ICityDataRepository
{
    private readonly ILogger<CityDataRepository> _logger;
    private readonly IConfiguration _configuration;
    private readonly SolarWatchContext _dbContext;
    
    public CityDataRepository(ILogger<CityDataRepository> logger, IConfiguration configuration, SolarWatchContext dbContext)
    {
        _logger = logger;
        _configuration = configuration;
        _dbContext = dbContext;
    }

    public async Task<List<City>> GetAllCityData()
    {
        return await _dbContext.CityData.ToListAsync();
    }

    public async Task<City?> GetCityData(string city)
    {
        return await _dbContext.CityData.FirstOrDefaultAsync(c => c.CityName == city);
    }

    public async Task<City?> GetCityDataById(int id)
    {
        return await _dbContext.CityData.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddCityData(City city)
    {
        var cityDataToAdd = _dbContext.CityData.FirstOrDefault(c => c.CityName == city.CityName);
        
        if (cityDataToAdd != null)
        {
            throw new Exception("City already exists.");
        }
        
        await _dbContext.CityData.AddAsync(city);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<City> UpdateCityData(City city)
    {
        var cityDataToUpdate = _dbContext.CityData.FirstOrDefault(c => c.Id == city.Id);
        if (cityDataToUpdate is null)
        {
            throw new Exception("City not found.");
        }
        
        _dbContext.CityData.Entry(cityDataToUpdate).CurrentValues.SetValues(city);
        await _dbContext.SaveChangesAsync();
        
        return await _dbContext.CityData.FirstAsync(c => c.CityName == city.CityName) ?? throw new Exception("City not found.");
    }

    public async Task DeleteCityData(int id)
    {
        var cityDataToDelete = await  _dbContext.CityData.FirstOrDefaultAsync(c => c.Id == id);
        
        if (cityDataToDelete is null)
        {
            throw new Exception("City not found.");
        }
        
        _dbContext.CityData.Remove(cityDataToDelete);
        await _dbContext.SaveChangesAsync();
    }

    public async Task SaveCityData(City city)
    {
        var cityDataEntity = await _dbContext.CityData.FirstOrDefaultAsync(c => c.CityName == city.CityName);

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
            await _dbContext.CityData.AddAsync(cityDataEntity);
        }
        else
        {
            _logger.LogInformation($"City: {city.CityName} already exists in the database.");
        }

        Console.WriteLine(_dbContext.Database.ProviderName);
        await _dbContext.SaveChangesAsync();
    }
}