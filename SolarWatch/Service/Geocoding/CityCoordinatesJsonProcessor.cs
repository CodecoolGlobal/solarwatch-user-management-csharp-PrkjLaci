using System;
using System.Linq;
using System.Text.Json;
using SolarWatch.Models;

namespace SolarWatch.Service.Geocoding
{
    public class CityCoordinatesJsonProcessor : ICityCoordinatesJsonProcessor
    {
        public CityCoordinate Process(string data)
        {
            JsonDocument json = JsonDocument.Parse(data);
            JsonElement root = json.RootElement;
            
            var coordElement = root.GetProperty("coord");

            CityCoordinate coordinates = new CityCoordinate
            {
                Latitude = coordElement.GetProperty("lat").GetDouble(),
                Longitude = coordElement.GetProperty("lon").GetDouble()
            };
            
            return coordinates;
        }
    }
}