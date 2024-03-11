﻿using Microsoft.AspNetCore.Mvc;
using SolarWatch.Service.Geocoding;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class CityDataController : ControllerBase
{
    private readonly ILogger<CityDataController> _logger;
    private readonly IGeocodingApiProvider _geocodingApiProvider;
    private readonly ICityCoordinatesJsonProcessor _cityCoordinatesJsonProcessor;
    
    public CityDataController(ILogger<CityDataController> logger, 
        IGeocodingApiProvider geocodingApiProvider,
        ICityCoordinatesJsonProcessor cityCoordinatesJsonProcessor)
    {
        _logger = logger;
        _geocodingApiProvider = geocodingApiProvider;
        _cityCoordinatesJsonProcessor = cityCoordinatesJsonProcessor;
    }
    
    [HttpGet("GetCityCoordinates")]
    public async Task<ActionResult> GetCityCoordinates(string city)
    {
        try
        {
            var cityCoordinates = await _geocodingApiProvider.GetCityCoordinates(city);
            return Ok(_cityCoordinatesJsonProcessor.Process(cityCoordinates));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting city coordinates");
            return BadRequest("Error getting city coordinates");
        }
    }
}