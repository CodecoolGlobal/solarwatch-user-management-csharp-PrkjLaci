using Microsoft.AspNetCore.Mvc;
using SolarWatch.Service.Geocoding;

namespace SolarWatch.Controllers;

[ApiController]
[Route("[controller]")]
public class CityCoordinateController : ControllerBase
{
    private readonly ILogger<CityCoordinateController> _logger;
    private readonly IGeocodingApiProvider _geocodingApiProvider;
    
    public CityCoordinateController(ILogger<CityCoordinateController> logger, IGeocodingApiProvider geocodingApiProvider)
    {
        _logger = logger;
        _geocodingApiProvider = geocodingApiProvider;
    }
    
    [HttpGet("GetCityCoordinates")]
    public IActionResult GetCityCoordinates(string city)
    {
        try
        {
            var cityCoordinates = _geocodingApiProvider.GetCityCoordinates(city);
            _logger.LogInformation("City coordinates: {cityCoordinates}", cityCoordinates);
            return Ok(cityCoordinates);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting city coordinates");
            return BadRequest("Error getting city coordinates");
        }
    }
}