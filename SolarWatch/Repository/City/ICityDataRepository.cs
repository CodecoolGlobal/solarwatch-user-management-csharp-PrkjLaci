using SolarWatch.Models;

namespace SolarWatch.Repository.City;

public interface ICityDataRepository
{
    public CityData? GetCityData(string city);
    public void SaveCityData(CityData cityData);
}