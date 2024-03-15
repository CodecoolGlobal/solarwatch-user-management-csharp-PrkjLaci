namespace SolarWatch.Models;

public class City
{
    public int Id { get; set; }
    public string? CityName { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public List<SunsetSunriseTime?> SunsetSunriseTime { get; set; }
}