namespace SolarWatch.Service.SunsetSunRise;

public interface ISunsetSunriseApiProvider
{ 
    string GetSunsetSunrise(string city, string date);
}