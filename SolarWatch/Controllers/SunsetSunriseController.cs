using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models;
using SolarWatch.Repository.CityRepository;
using SolarWatch.Repository.SunsetSunriseRepository;
using SolarWatch.Service.Geocoding;
using SolarWatch.Service.SunsetSunRise;

namespace SolarWatch.Controllers;
[ApiController]
[Route("[controller]")]
public class SunsetSunriseController : ControllerBase
{
    private readonly ILogger<SunsetSunriseController> _logger;
    private readonly ISunsetSunriseApiProvider _sunsetSunriseApiProvider;
    private readonly ISunsetSunriseJsonProcessor _sunsetSunriseJsonProcessor;
    private readonly ISunsetSunriseRepository _sunsetSunriseRepository;
    private readonly ICityDataRepository _cityDataRepository;
    private readonly IGeocodingApiProvider _geocodingApiProvider;
    private readonly ICityCoordinatesJsonProcessor _cityCoordinatesJsonProcessor;
    
    public SunsetSunriseController(ILogger<SunsetSunriseController> logger, 
        ISunsetSunriseApiProvider sunsetSunriseApiProvider,
        ISunsetSunriseJsonProcessor sunsetSunriseJsonProcessor,
        ISunsetSunriseRepository sunsetSunriseRepository,
        ICityDataRepository cityDataRepository,
        IGeocodingApiProvider geocodingApiProvider,
        ICityCoordinatesJsonProcessor cityCoordinatesJsonProcessor)
    {
        _logger = logger;
        _sunsetSunriseApiProvider = sunsetSunriseApiProvider;
        _sunsetSunriseJsonProcessor = sunsetSunriseJsonProcessor;
        _sunsetSunriseRepository = sunsetSunriseRepository;
        _cityDataRepository = cityDataRepository;
        _geocodingApiProvider = geocodingApiProvider;
        _cityCoordinatesJsonProcessor = cityCoordinatesJsonProcessor;
    }
    
    [Authorize]
    [HttpGet("GetSunsetSunrise")]
    public async Task<ActionResult> GetSunsetSunrise(string city, string date)
    {
        try
        {
            var sunsetSunriseTime = await _sunsetSunriseRepository.GetSunsetSunrise(city, date);
            var cityData = await _cityDataRepository.GetCityData(city);
            
            if (sunsetSunriseTime is not null && cityData is not null)
            {
                sunsetSunriseTime.City = cityData;
                return Ok(new { message = "Successfully get the sunset and the sunrise.", data = sunsetSunriseTime });
            }
            
            var sunsetSunriseFromApi = await _sunsetSunriseApiProvider.GetSunsetSunrise(city, date);
            var sunsetSunriseEntity = _sunsetSunriseJsonProcessor.Process(sunsetSunriseFromApi);
            sunsetSunriseEntity.Date = date;
            
            var cityFromApi = await _geocodingApiProvider.GetCityCoordinates(city);
            var cityEntity = _cityCoordinatesJsonProcessor.Process(cityFromApi);
            
            await _sunsetSunriseRepository.SaveSunsetSunrise(cityEntity, sunsetSunriseEntity);
            sunsetSunriseTime = sunsetSunriseEntity;
            
            return Ok(new { message = "Successfully get the sunset and the sunrise.", data = sunsetSunriseTime});
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting sunset and sunrise data");
            return BadRequest(new { message = "Error getting sunset and sunrise data" });
        }
    }
    [Authorize (Roles = "Admin")]
    [HttpGet("GetAllSunsetSunrise")]
    public async Task<ActionResult> GetAllSunsetSunrise()
    {
        try
        {
            var sunsetSunriseTimes = await _sunsetSunriseRepository.GetAllSunsetSunrise();
            return Ok(new { message = "Successfully get all sunset and sunrise.", data = sunsetSunriseTimes });

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting all sunset and sunrise data");
            return BadRequest(new { message = "Error getting all sunset and sunrise data" });
        }
    }
    
    [HttpPost("AddSunsetSunrise"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> AddSunsetSunrise(SunsetSunriseTime sunsetSunrise)
    {
        try
        {
            var sunsetSunriseToAdd = await _sunsetSunriseRepository.AddSunsetSunrise(sunsetSunrise);
            return Ok(new { message = "Sunset and sunrise added.", data = sunsetSunriseToAdd });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Sunset and sunrise already exists.");
            return BadRequest(new { message = "Sunset and sunrise already exists." });
        }
    }
    
    [HttpPatch("UpdateSunsetSunrise"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateSunsetSunrise(SunsetSunriseTime sunsetSunrise)
    {
        try
        {
            var updatedSunsetSunrise = await _sunsetSunriseRepository.UpdateSunsetSunrise(sunsetSunrise);
            return Ok(new { message = "Sunset and sunrise updated.", data = updatedSunsetSunrise });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Sunset and sunrise not found.");
            return NotFound(new { message = "Sunset and sunrise not found." });
        }
    }
    
    [HttpDelete("DeleteSunsetSunrise/{id}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteSunsetSunrise(int id)
    {
        try
        {
            await _sunsetSunriseRepository.DeleteSunsetSunrise(id);
            return Ok(new { message = "Sunset and sunrise deleted." });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Sunset and sunrise not found.");
            return NotFound(new { message = "Sunset and sunrise not found." });
        }
    }

}