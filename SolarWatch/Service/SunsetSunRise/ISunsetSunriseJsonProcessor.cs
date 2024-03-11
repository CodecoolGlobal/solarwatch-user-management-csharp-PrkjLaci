using SolarWatch.Models;

namespace SolarWatch.Service.SunsetSunRise;

public interface ISunsetSunriseJsonProcessor
{
    SunsetSunriseTime Process(string data);
}