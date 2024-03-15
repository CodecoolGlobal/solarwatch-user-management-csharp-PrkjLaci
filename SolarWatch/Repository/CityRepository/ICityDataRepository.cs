namespace SolarWatch.Repository.CityRepository;
using Models;
public interface ICityDataRepository
{
    public Task<City?> GetCityData(string city);
    void AddCityData(City city);
    public Task SaveCityData(City city);
}