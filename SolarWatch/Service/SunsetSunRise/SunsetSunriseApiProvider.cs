using System.Net;
using SolarWatch.Service.Geocoding;

namespace SolarWatch.Service.SunsetSunRise;

public class SunsetSunriseApiProvider : ISunsetSunriseApiProvider
{
    private readonly ILogger<SunsetSunriseApiProvider> _logger;
    private readonly IGeocodingApiProvider _geocodingApiProvider;
    private readonly ICityCoordinatesJsonProcessor _cityCoordinatesJsonProcessor;
    
    public SunsetSunriseApiProvider(ILogger<SunsetSunriseApiProvider> logger, 
        IGeocodingApiProvider geocodingApiProvider,
        ICityCoordinatesJsonProcessor cityCoordinatesJsonProcessor)
    {
        _logger = logger;
        _geocodingApiProvider = geocodingApiProvider;
        _cityCoordinatesJsonProcessor = cityCoordinatesJsonProcessor;
    }
    
    public async Task<string> GetSunsetSunrise(string city, string date)
    {
        var cityData = await _geocodingApiProvider.GetCityCoordinates(city);
        var cityCoordinates = _cityCoordinatesJsonProcessor.Process(cityData);

        var url =
            $"https://api.sunrise-sunset.org/json?lat={cityCoordinates.Latitude}&lng={cityCoordinates.Longitude}&date={date}";
        using var client = new HttpClient();
        
        _logger.LogInformation($"Calling OpenWeather API with url: {url}");
        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}