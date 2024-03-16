using SolarWatch.Models;

namespace SolarWatch.Repository.SunsetSunriseRepository;

public interface ISunsetSunriseRepository
{
    public Task<SunsetSunriseTime?> GetSunsetSunrise(string city, string date);
    public Task AddSunsetSunrise(SunsetSunriseTime sunsetSunrise, int cityId);

    public Task<SunsetSunriseTime>
        UpdateSunsetSunrise(SunsetSunriseTime sunsetSunrise, int cityId, int sunsetSunriseId);
    public Task DeleteSunsetSunrise(int id);
    public Task SaveSunsetSunrise(City? city, SunsetSunriseTime sunsetSunrise);
}