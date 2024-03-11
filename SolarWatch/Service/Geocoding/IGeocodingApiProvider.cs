namespace SolarWatch.Service.Geocoding;

public interface IGeocodingApiProvider
{
    string GetCityCoordinates(string city);
}