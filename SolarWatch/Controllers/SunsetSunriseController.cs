using Microsoft.AspNetCore.Mvc;
using SolarWatch.Service.Geocoding;
using SolarWatch.Service.SunsetSunRise;

namespace SolarWatch.Controllers;
[ApiController]
[Route("[controller]")]
public class SunsetSunriseController : ControllerBase
{
    private readonly ILogger<SunsetSunriseController> _logger;
    
    private readonly IGeocodingApiProvider _geocodingApiProvider;
    private readonly ICityCoordinatesJsonProcessor _cityCoordinatesJsonProcessor;

    private readonly ISunsetSunriseApiProvider _sunsetSunriseApiProvider;
    private readonly ISunsetSunriseJsonProcessor _sunsetSunriseJsonProcessor;
    
    public SunsetSunriseController(ILogger<SunsetSunriseController> logger, 
        IGeocodingApiProvider geocodingApiProvider,
        ICityCoordinatesJsonProcessor cityCoordinatesJsonProcessor,
        ISunsetSunriseApiProvider sunsetSunriseApiProvider,
        ISunsetSunriseJsonProcessor sunsetSunriseJsonProcessor)
    {
        _logger = logger;
        _geocodingApiProvider = geocodingApiProvider;
        _cityCoordinatesJsonProcessor = cityCoordinatesJsonProcessor;
        _sunsetSunriseApiProvider = sunsetSunriseApiProvider;
        _sunsetSunriseJsonProcessor = sunsetSunriseJsonProcessor;
    }
    
    [HttpGet("GetSunsetSunrise")]
    public async Task<ActionResult> GetSunsetSunrise(string city, string date)
    {
        try
        {
            var sunsetSunriseData = await _sunsetSunriseApiProvider.GetSunsetSunrise(city, date);
            return Ok(_sunsetSunriseJsonProcessor.Process(sunsetSunriseData));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting sunset sunrise data");
            return BadRequest("Error getting sunset sunrise data");
        }
    }
}