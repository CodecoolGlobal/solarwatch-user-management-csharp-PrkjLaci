using System.Text.Json;
using SolarWatch.Models;

namespace SolarWatch.Service.SunsetSunRise;

public class SunsetSunriseJsonProcessor : ISunsetSunriseJsonProcessor
{
    public SunsetSunriseTime Process(string data)
    {
        JsonDocument json = JsonDocument.Parse(data);
        JsonElement root = json.RootElement;

        var results = root.GetProperty("results");

        SunsetSunriseTime sunsetSunriseTime = new SunsetSunriseTime
        {
            Sunrise = results.GetProperty("sunrise").ToString(),
            Sunset = results.GetProperty("sunset").ToString()
        };

        return sunsetSunriseTime;
    }
}