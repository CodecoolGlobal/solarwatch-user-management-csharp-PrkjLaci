using SolarWatch.Models;

namespace SolarWatch.Repository.SunsetSunriseRepository;

public interface ISunsetSunriseRepository
{
    public Task<SunsetSunriseTime?> GetSunsetSunrise(string city, string date);
    public Task SaveSunsetSunrise(City? city, SunsetSunriseTime sunsetSunrise);
}