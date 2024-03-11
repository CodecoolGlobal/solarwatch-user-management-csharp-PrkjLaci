namespace SolarWatch.Service.Geocoding;

public interface IGeocodingApiProvider
{
    Task<string> GetCityCoordinates(string city);
}