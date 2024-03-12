namespace SolarWatch.Repository.CityRepository;

public interface ICityDataRepository
{
    public Models.City? GetCityData(string city);
    public void SaveCityData(Models.City city);
}