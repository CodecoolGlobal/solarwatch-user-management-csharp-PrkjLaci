using SolarWatch.Models;

namespace SolarWatch.Contracts.SunsetSunrise;

public class SingleSunsetSunriseResponseData
{
    public int Id { get; set; }
    public string Message { get; set; }
    public SunsetSunriseTime Data { get; set; }
}
