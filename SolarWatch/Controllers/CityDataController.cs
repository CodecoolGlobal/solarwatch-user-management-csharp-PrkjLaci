﻿using Microsoft.AspNetCore.Mvc;
using SolarWatch.Repository.CityRepository;
using SolarWatch.Service.Geocoding;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class CityDataController : ControllerBase
{
    private readonly ILogger<CityDataController> _logger;
    private readonly IGeocodingApiProvider _geocodingApiProvider;
    private readonly ICityCoordinatesJsonProcessor _cityCoordinatesJsonProcessor;
    private readonly ICityDataRepository _cityDataRepository;
    
    public CityDataController(ILogger<CityDataController> logger, 
        IGeocodingApiProvider geocodingApiProvider,
        ICityCoordinatesJsonProcessor cityCoordinatesJsonProcessor,
        ICityDataRepository cityDataRepository)
    {
        _logger = logger;
        _geocodingApiProvider = geocodingApiProvider;
        _cityCoordinatesJsonProcessor = cityCoordinatesJsonProcessor;
        _cityDataRepository = cityDataRepository;
    }
    
    [HttpGet("GetCityCoordinates")]
    public async Task<ActionResult> GetCityCoordinates(string city)
    {
        try
        {
            var cityData = _cityDataRepository.GetCityData(city);
            
            if(cityData != null)
            {
                return Ok(cityData);
            }
            var cityDataFromApi = await _geocodingApiProvider.GetCityCoordinates(city);
            var cityEntity = _cityCoordinatesJsonProcessor.Process(cityDataFromApi);
            
            _cityDataRepository.SaveCityData(cityEntity);
            
            return Ok(cityEntity);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting city coordinates");
            return BadRequest("Error getting city coordinates");
        }
    }
}