using SolarWatch.Models;

namespace SolarWatch.Service.Geocoding;

public interface ICityCoordinatesJsonProcessor
{
    City Process(string data);
}