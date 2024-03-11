using System;
using System.Linq;
using System.Text.Json;
using SolarWatch.Models;

namespace SolarWatch.Service.Geocoding
{
    public class CityCoordinatesJsonProcessor : ICityCoordinatesJsonProcessor
    {
        public CityData Process(string data)
        {
            JsonDocument json = JsonDocument.Parse(data);
            JsonElement root = json.RootElement;
          
            JsonElement cityElement = root[0];

            CityData cityData = new CityData
            {
                CityName = cityElement.GetProperty("name").GetString(),
                Latitude = cityElement.GetProperty("lat").GetDouble(),
                Longitude = cityElement.GetProperty("lon").GetDouble(),
                Country = cityElement.GetProperty("country").GetString(),
            };
            
            return coordinates;
        }
    }
}