using System.Net;
using System.Net.Http.Headers;
using System.Text;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using SolarWatch.Contracts;
using SolarWatch.Contracts.CityData;
using SolarWatch.Contracts.SunsetSunrise;
using SolarWatch.IntegrationTests.Authentication;
using SolarWatch.Models;
using Xunit.Abstractions;

namespace SolarWatch.IntegrationTests.ControllerTests;

[Collection("IntegrationTests")]
public class PatchEndpointTests : IClassFixture<SolarWatchWebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;
    
    public PatchEndpointTests(SolarWatchWebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _output = output;
    }

    //Sunset sunrise tests

    [Fact]
    public async Task UpdateSunsetSunrise_Returns_SunsetSunriseData()
    {
        const string url = "/SunsetSunrise/UpdateSunsetSunrise";
        var token = new TestJwtToken().WithRole("Admin").WithName("testAdmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var sunsetSunrise = new SunsetSunriseTime
        {
            Id = 1,
            Date = "2024-04-10",
            Sunset = "18:03",
            Sunrise = "05:57",
            CityId = 1
        };

        var sunsetSunriseJson = JsonConvert.SerializeObject(sunsetSunrise);
        var content = new StringContent(sunsetSunriseJson, Encoding.UTF8, "application/json");

        var response = await _client.PatchAsync(url, content);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var sunsetSunriseResponse = JsonConvert.DeserializeObject<SingleSunsetSunriseResponseData>(responseString);

        sunsetSunriseResponse.Should().NotBeNull();
        sunsetSunriseResponse?.Data.Id.Should().Be(1);
        sunsetSunriseResponse?.Data.Date.Should().Be("2024-04-10");
        sunsetSunriseResponse?.Data.Sunset.Should().Be("18:03");
        sunsetSunriseResponse?.Data.Sunrise.Should().Be("05:57");
    }
    
    [Fact]
    public async Task UpdateSunsetSunrise_Returns_NotFound()
    {
        const string url = "/SunsetSunrise/UpdateSunsetSunrise";
        var token = new TestJwtToken().WithRole("Admin").WithName("testAdmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var sunsetSunrise = new SunsetSunriseTime
        {
            Id = 3,
            Date = "2024-04-12",
            Sunset = "18:02",
            Sunrise = "05:58",
            CityId = 1
        };

        var sunsetSunriseJson = JsonConvert.SerializeObject(sunsetSunrise);
        var content = new StringContent(sunsetSunriseJson, Encoding.UTF8, "application/json");

        var response = await _client.PatchAsync(url, content);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    //City tests
    
    [Fact]
    public async Task UpdateCity_Returns_CityData()
    {
        const string url = "/CityData/UpdateCityData";
        var token = new TestJwtToken().WithRole("Admin").WithName("testAdmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var city = new City
        {
            Id = 1,
            CityName = "Budapest",
            Latitude = 47.4979,
            Longitude = 19.0402,
            State = null,
            Country = "Hungary",
            SunsetSunriseTime = new List<SunsetSunriseTime>()
        };

        var cityJson = JsonConvert.SerializeObject(city);
        var content = new StringContent(cityJson, Encoding.UTF8, "application/json");

        var response = await _client.PatchAsync(url, content);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        
        _output.WriteLine(responseString);
        
        var cityResponse = JsonConvert.DeserializeObject<SingleCityResponseData>(responseString);
        
        cityResponse.Should().NotBeNull();
        cityResponse?.Data.Id.Should().Be(1);
        cityResponse?.Data.CityName.Should().Be("Budapest");
        cityResponse?.Data.Latitude.Should().Be(47.4979);
        cityResponse?.Data.Longitude.Should().Be(19.0402);
        cityResponse?.Data.State.Should().BeNull();
        cityResponse?.Data.Country.Should().Be("Hungary");
    }

    [Fact]
    public async Task UpdateCityData_Returns_NotFound()
    {
        const string url = "/CityData/UpdateCityData";
        var token = new TestJwtToken().WithRole("Admin").WithName("testAdmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var city = new City
        {
            Id = 3,
            CityName = "Budapest",
            Latitude = 47.4979,
            Longitude = 19.0402,
            State = null,
            Country = "Hungary",
            SunsetSunriseTime = new List<SunsetSunriseTime>()
        };
        
        var cityJson = JsonConvert.SerializeObject(city);
        var content = new StringContent(cityJson, Encoding.UTF8, "application/json");
        
        var response = await _client.PatchAsync(url, content);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
}