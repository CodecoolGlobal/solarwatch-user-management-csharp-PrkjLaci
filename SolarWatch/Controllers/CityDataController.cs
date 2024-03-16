using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolarWatch.Models;
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
    
    [HttpGet("GetCityCoordinates"), Authorize(Roles = "Admin, User")]
    public async Task<ActionResult> GetCityCoordinates(string city)
    {
        try
        {
            var cityData = await _cityDataRepository.GetCityData(city);
            
            if(cityData != null)
            {
                return Ok(cityData);
            }
            var cityDataFromApi = await _geocodingApiProvider.GetCityCoordinates(city);
            var cityEntity = _cityCoordinatesJsonProcessor.Process(cityDataFromApi);
            
            await _cityDataRepository.SaveCityData(cityEntity);
            
            return Ok(new { message = $"Successfully get city data.", data = cityEntity });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting city coordinates");
            return BadRequest(new { message = "Error getting city coordinates" });
        }
    }
    
    [HttpPost("AddCityData"), Authorize(Roles = "Admin")] 
    public async Task<ActionResult> AddCityData(City cityData)
    {
        try
        {
            await _cityDataRepository.AddCityData(cityData);
            await _cityDataRepository.SaveCityData(cityData);
            return Ok(new { message = "City data added.", data = cityData });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "City data already exists.");
            return BadRequest(new { message = "City data already exists." });
        }
    }
    
    [HttpPatch("UpdateCityData"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateCityData(City cityData)
    {
        try
        {
            var updatedCityData = await _cityDataRepository.UpdateCityData(cityData);
            await _cityDataRepository.SaveCityData(updatedCityData);
            return Ok(new { message = "City data updated.", data = updatedCityData });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "City data not found.");
            return NotFound(new { message = "City data not found." });
        }
    }
    
    [HttpDelete("DeleteCityData/{cityId}"), Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteCityData(int cityId)
    {
        try
        {
            await _cityDataRepository.DeleteCityData(cityId);
            return Ok(new { message = "City data deleted." });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "City data not found.");
            return NotFound(new { message = "City data not found." });
        }
    }
    
}