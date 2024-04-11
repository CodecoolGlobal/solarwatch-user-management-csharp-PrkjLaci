using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SolarWatch.Data;
using SolarWatch.IntegrationTests.Authentication;

namespace SolarWatch.IntegrationTests.ControllerTests;

[Collection("IntegrationTests")]
public class BasicTests : IClassFixture<SolarWatchWebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    
    public BasicTests(SolarWatchWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }
    
    [Theory]
    [InlineData("/CityData/GetAllCityData")]
    [InlineData("/CityData/GetCityCoordinates?city=Budapest")]
    [InlineData("/CityData/GetCityDataById/1")]
    [InlineData("/SunsetSunrise/GetSunsetSunrise?city=Budapest&date=2024-04-10")]
    [InlineData("/SunsetSunrise/GetAllSunsetSunrise")]
    public async Task Get_Should_Reject_Unauthenticated_Request(string url)
    {
        var response = await _client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Theory]
    [InlineData("/CityData/GetCityCoordinates?city=Budapest")]
    [InlineData("/SunsetSunrise/GetSunsetSunrise?city=Budapest&date=2024-04-10")]
    public async Task Get_Should_Accept_Authenticated_Request(string url)
    {
        var token = new TestJwtToken().WithRole("User").WithName("testUser").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Theory]
    [InlineData("/CityData/GetAllCityData")]
    [InlineData("/CityData/GetCityDataById/1")]
    [InlineData("/SunsetSunrise/GetAllSunsetSunrise")]
    public async Task Get_Should_Reject_Unauthorized_Request(string url)
    {
        var token = new TestJwtToken().WithRole("User").WithName("testUser").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
    
    [Theory]
    [InlineData("/CityData/GetCityCoordinates?city=Budapest")]
    [InlineData("/SunsetSunrise/GetSunsetSunrise?city=Budapest&date=2024-04-10")]
    [InlineData("/CityData/GetAllCityData")]
    [InlineData("/CityData/GetCityDataById/1")]
    [InlineData("/SunsetSunrise/GetAllSunsetSunrise")]
    public async Task Get_Should_Accept_Admin_Request(string url)
    {
        var token = new TestJwtToken().WithRole("Admin").WithName("testAdmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
