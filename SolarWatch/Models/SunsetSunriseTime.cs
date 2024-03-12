namespace SolarWatch.Models;

public class SunsetSunriseTime
{
    public int Id { get; set; }
    public string? Date { get; set; }
    public string? Sunrise { get; set; }
    public string? Sunset { get; set; }
    public int CityId { get; set; }
    public City? City { get; set; }
}