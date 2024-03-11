using System.ComponentModel.DataAnnotations.Schema;

namespace SolarWatch.Models;

public class CityData
{
    public int Id { get; set; }
    public string? CityName { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    
    [ForeignKey("SunsetSunriseTime")]
    public int SunsetSunriseTimeId { get; set; }
    public SunsetSunriseTime? SunsetSunriseTime { get; set; }
}