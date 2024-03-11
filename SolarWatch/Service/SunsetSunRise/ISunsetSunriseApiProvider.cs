namespace SolarWatch.Service.SunsetSunRise;

public interface ISunsetSunriseApiProvider
{ 
    Task<string> GetSunsetSunrise(string city, string date);
}