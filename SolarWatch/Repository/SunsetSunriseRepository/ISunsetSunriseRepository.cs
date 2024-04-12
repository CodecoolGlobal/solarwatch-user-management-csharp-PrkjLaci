using SolarWatch.Models;

namespace SolarWatch.Repository.SunsetSunriseRepository;

public interface ISunsetSunriseRepository
{
    public Task<SunsetSunriseTime?> GetSunsetSunrise(string city, string date);
    public Task<List<SunsetSunriseTime>> GetAllSunsetSunrise();
    public Task<SunsetSunriseTime> AddSunsetSunrise(SunsetSunriseTime sunsetSunrise);

    public Task<SunsetSunriseTime>
        UpdateSunsetSunrise(SunsetSunriseTime sunsetSunrise);
    public Task DeleteSunsetSunrise(int id);
    public Task SaveSunsetSunrise(City? city, SunsetSunriseTime sunsetSunrise);
}