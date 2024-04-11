using System.Net;
using System.Net.Http.Headers;
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
public class GetEndpointTests : IClassFixture<SolarWatchWebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;
    
    public GetEndpointTests(SolarWatchWebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _output = output;
    }

    //Sunset sunrise tests
    
    [Fact]
    public async Task GetSunsetSunrise_Returns_SunsetSunriseData()
    {
        const string url = "/SunsetSunrise/GetSunsetSunrise?city=Budapest&date=2024-04-10";
        var token = new TestJwtToken().WithRole("User").WithName("testUser").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        var responseString = await response.Content.ReadAsStringAsync();
        var sunsetSunrise = JsonConvert.DeserializeObject<SingleSunsetSunriseResponseData>(responseString);
        
        sunsetSunrise.Should().NotBeNull();
        sunsetSunrise?.Data.Id.Should().Be(1);
        sunsetSunrise?.Data.Date.Should().Be("2024-04-10");
        sunsetSunrise?.Data.City?.CityName.Should().Be("Budapest");
    }
    
    [Fact]
    public async Task GetAllSunsetSunrise_Returns_AllSunsetSunrise()
    {
        const string url = "/SunsetSunrise/GetAllSunsetSunrise";
        var token = new TestJwtToken().WithRole("Admin").WithName("testAdmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        var responseString = await response.Content.ReadAsStringAsync();
        var sunsetSunriseList = JsonConvert.DeserializeObject<SunsetSunriseResponseData>(responseString)?.Data;
        
        sunsetSunriseList.Should().NotBeNull();
        sunsetSunriseList.Should().HaveCount(2);
        sunsetSunriseList?[0].Id.Should().Be(1);
        sunsetSunriseList?[1].Id.Should().Be(2);
    }
    
    //City tests
    
    [Fact]
    public async Task GetCityDataById_Returns_CityData()
    {
        const string url = "/CityData/GetCityDataById/1";
        var token = new TestJwtToken().WithRole("Admin").WithName("testAdmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        var responseString = await response.Content.ReadAsStringAsync();
        var cityData = JsonConvert.DeserializeObject<City>(responseString);
        
        cityData.Should().NotBeNull();
        cityData.Id.Should().Be(1);
    }
    
    [Fact]
    public async Task GetCityDataById_Returns_NotFound()
    {
        const string url = "/CityData/GetCityDataById/2";
        var token = new TestJwtToken().WithRole("Admin").WithName("testAdmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    [Fact]
    public async Task GetCityCoordinates_Returns_CityCoordinates()
    {
        const string url = "/CityData/GetCityCoordinates?city=Budapest";
        var token = new TestJwtToken().WithRole("Admin").WithName("testAdmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        var responseString = await response.Content.ReadAsStringAsync();
        var cityCoordinates = JsonConvert.DeserializeObject<City>(responseString);
        
        cityCoordinates.Should().NotBeNull();
        cityCoordinates?.CityName.Should().Be("Budapest");
        cityCoordinates?.Latitude.Should().Be(47.4979);
        cityCoordinates?.Longitude.Should().Be(19.0402);
    }
    
    [Fact]
    public async Task GetAllCityData_Returns_AllCityData()
    {
        const string url = "/CityData/GetAllCityData";
        var token = new TestJwtToken().WithRole("Admin").WithName("testAdmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        
        var responseString = await response.Content.ReadAsStringAsync();
        var cityDataList = JsonConvert.DeserializeObject<List<City>>(responseString);
        
        _output.WriteLine(responseString);
        
        cityDataList.Should().NotBeNull();
        cityDataList?[0].Id.Should().Be(1);
    }
}