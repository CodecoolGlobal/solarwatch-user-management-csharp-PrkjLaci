namespace SolarWatch.Repository.CityRepository;
using Models;
public interface ICityDataRepository
{
    public Task<List<City>> GetAllCityData();
    public Task<City?> GetCityData(string city);
    public Task<City?> GetCityDataById(int id);
    public Task AddCityData(City city);
    public Task<City> UpdateCityData(City city);
    public Task DeleteCityData(int id);
    public Task SaveCityData(City city);
}