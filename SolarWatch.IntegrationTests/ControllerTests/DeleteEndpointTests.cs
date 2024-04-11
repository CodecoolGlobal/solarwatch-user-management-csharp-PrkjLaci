using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using SolarWatch.IntegrationTests.Authentication;
using Xunit.Abstractions;

namespace SolarWatch.IntegrationTests.ControllerTests;

[Collection("IntegrationTests")]
public class DeleteEndpointTests : IClassFixture<SolarWatchWebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;
    private readonly ITestOutputHelper _output;
    
    public DeleteEndpointTests(SolarWatchWebApplicationFactory<Program> factory, ITestOutputHelper output)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _output = output;
    }
    
    //Sunset sunrise tests
    
    [Fact]
    public async Task DeleteSunsetSunrise_Returns_SunsetSunriseData()
    {
        Task.WaitAny();
        const string url = "/SunsetSunrise/DeleteSunsetSunrise/2";
        var token = new TestJwtToken().WithRole("Admin").WithName("testAdmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task DeleteSunsetSunrise_Returns_NotFound()
    {
        Task.WaitAny();
        const string url = "/SunsetSunrise/DeleteSunsetSunrise/3";
        var token = new TestJwtToken().WithRole("Admin").WithName("testAdmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
    
    
    //City data tests
    
    [Fact]
    public async Task DeleteCityData_Returns_CityData()
    {
        Task.WaitAny();
        const string url = "/CityData/DeleteCityData/1";
        var token = new TestJwtToken().WithRole("Admin").WithName("testAdmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync(url);
        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task DeleteCityData_Returns_NotFound()
    {
        const string url = "/CityData/DeleteCityData/2";
        var token = new TestJwtToken().WithRole("Admin").WithName("testAdmin").Build();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.DeleteAsync(url);
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}