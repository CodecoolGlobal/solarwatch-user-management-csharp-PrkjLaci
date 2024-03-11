using SolarWatch.Models;

namespace SolarWatch.Service.Geocoding;

public interface ICityCoordinatesJsonProcessor
{
    CityCoordinate Process(string data);
}