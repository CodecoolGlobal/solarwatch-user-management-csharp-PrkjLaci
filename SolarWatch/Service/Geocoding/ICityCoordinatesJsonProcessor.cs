using SolarWatch.Models;

namespace SolarWatch.Service.Geocoding;

public interface ICityCoordinatesJsonProcessor
{
    CityData Process(string data);
}