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
public class PostEndpointTests : IClassFixture<SolarWatchWebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;
    
    public PostEndpointTests(SolarWatchWebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _output = output;
    }

    //Sunset sunrise tests
    
    [Fact]
    public async Task AddSunsetSunrise_Returns_SunsetSunriseData()
    {
        const string url = "/SunsetSunrise/AddSunsetSunrise";
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
        
        var response = await _client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        
        var responseString = await response.Content.ReadAsStringAsync();
        var sunsetSunriseResponse = JsonConvert.DeserializeObject<SingleSunsetSunriseResponseData>(responseString);
        
        sunsetSunriseResponse.Should().NotBeNull();
        sunsetSunriseResponse?.Data.Id.Should().Be(3);
        sunsetSunriseResponse?.Data.Date.Should().Be("2024-04-12");
        sunsetSunriseResponse?.Data.City?.CityName.Should().Be("Budapest");
    }

    [Fact]
    public async Task AddSunsetSunrise_Return_BadRequest()
    {
        var url = "/SunsetSunrise/AddSunsetSunrise";
        var token = new TestJwtToken().WithRole("Admin").WithName("testAdmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var sunsetSunrise = new SunsetSunriseTime
        {
            Id = 1,
            Date = "2024-04-12",
            Sunset = "18:02",
            Sunrise = "05:58",
            CityId = 2
        };
        
        var sunsetSunriseJson = JsonConvert.SerializeObject(sunsetSunrise);
        var content = new StringContent(sunsetSunriseJson, Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync(url, content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
    //City tests
    
    [Fact]
    public async Task AddCity_Returns_CityData()
    {
        const string url = "/CityData/AddCityData";
        var token = new TestJwtToken().WithRole("Admin").WithName("testAdmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var city = new City
        {
            Id = 2,
            CityName = "Debrecen",
            Latitude = 47.5316,
            Longitude = 21.6273,
            State = null,
            Country = "Hungary",
            SunsetSunriseTime = new List<SunsetSunriseTime>()
        };
        
        var cityJson = JsonConvert.SerializeObject(city);
        var content = new StringContent(cityJson, Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        
        var responseString = await response.Content.ReadAsStringAsync();
        
        _output.WriteLine(responseString);
        var cityResponse = JsonConvert.DeserializeObject<SingleCityResponseData>(responseString)?.Data;
        
        cityResponse.Should().NotBeNull();
        cityResponse?.Id.Should().Be(2);
        cityResponse?.CityName.Should().Be("Debrecen");
    }
    
    [Fact]
    public async Task AddCity_Return_BadRequest()
    {
        var url = "/CityData/AddCityData";
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
        
        var response = await _client.PostAsync(url, content);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
    
}