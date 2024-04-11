using SolarWatch.Models;

namespace SolarWatch.Contracts.CityData;

public class CityResponseData
{
    public int Id { get; set; }
    public string Message { get; set; }
    public List<City> Data { get; set; }
}