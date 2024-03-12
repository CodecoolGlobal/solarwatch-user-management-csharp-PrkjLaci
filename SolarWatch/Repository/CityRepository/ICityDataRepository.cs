namespace SolarWatch.Repository.CityRepository;
using Models;
public interface ICityDataRepository
{
    public Task<City?> GetCityData(string city);
    public Task SaveCityData(Models.City city);
}