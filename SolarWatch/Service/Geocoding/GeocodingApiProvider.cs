using System.Net;

namespace SolarWatch.Service.Geocoding;

public class GeocodingApiProvider : IGeocodingApiProvider
{
    private readonly ILogger<GeocodingApiProvider> _logger;

    public GeocodingApiProvider(ILogger<GeocodingApiProvider> logger)
    {
        _logger = logger;
    }
    public string GetCityCoordinates(string city)
    {
        var apiKey = "ba0dd16bccd1e4d44407f12d6caf35da";
        var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}";

        using var client = new WebClient();
        
        _logger.LogInformation("Calling OpenWeather API with url: {url}", url);
        return client.DownloadString(url);
    }
}