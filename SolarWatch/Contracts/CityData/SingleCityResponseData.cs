using SolarWatch.Models;

namespace SolarWatch.Contracts.CityData;

public class SingleCityResponseData
{
    public int Id { get; set; }
    public string Message { get; set; }
    public City Data { get; set; }
}
