using SolarWatch.Models;

namespace SolarWatch.Contracts.SunsetSunrise;

public class SunsetSunriseResponseData
{
    public int Id { get; set; }
    public string Message { get; set; }
    public List<SunsetSunriseTime> Data { get; set; }
}