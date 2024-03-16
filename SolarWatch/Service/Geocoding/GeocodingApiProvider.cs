using System.Net;

namespace SolarWatch.Service.Geocoding;

public class GeocodingApiProvider : IGeocodingApiProvider
{
    private readonly ILogger<GeocodingApiProvider> _logger;
    private readonly IConfiguration _configuration;

    public GeocodingApiProvider(ILogger<GeocodingApiProvider> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }
    
    public async Task<string> GetCityCoordinates(string city)
    {
        var apiKey = _configuration["OPENWEATHER_API_KEY"];
        Console.WriteLine(apiKey);
        var url = $"https://api.openweathermap.org/geo/1.0/direct?q={city}&appid={apiKey}";

        using var client = new HttpClient();
        
        _logger.LogInformation("Calling OpenWeather API with url: {url}", url);
        var response = await client.GetAsync(url);
        return await response.Content.ReadAsStringAsync();
    }
}