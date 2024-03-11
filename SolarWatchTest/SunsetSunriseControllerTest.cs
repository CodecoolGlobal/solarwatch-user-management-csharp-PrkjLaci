using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Models;
using SolarWatch.Service.Geocoding;
using SolarWatch.Service.SunsetSunRise;

namespace SolarWatchTest;

[TestFixture]
public class SunsetSunriseControllerTest
{
    private Mock<ILogger<SunsetSunriseController>> _loggerMock;
    private Mock<ISunsetSunriseApiProvider> _sunsetSunriseProviderMock;
    private Mock<ISunsetSunriseJsonProcessor> _sunsetSunriseJsonProcessorMock;
    private Mock<IGeocodingApiProvider> _geocodingApiProviderMock;
    private Mock<ICityCoordinatesJsonProcessor> _cityCoordinatesJsonProcessorMock;
    private SunsetSunriseController _controller;
    
    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SunsetSunriseController>>();
        _sunsetSunriseProviderMock = new Mock<ISunsetSunriseApiProvider>();
        _sunsetSunriseJsonProcessorMock = new Mock<ISunsetSunriseJsonProcessor>();
        _geocodingApiProviderMock = new Mock<IGeocodingApiProvider>();
        _cityCoordinatesJsonProcessorMock = new Mock<ICityCoordinatesJsonProcessor>();
        _controller = new SunsetSunriseController(_loggerMock.Object, _geocodingApiProviderMock.Object,
            _cityCoordinatesJsonProcessorMock.Object, _sunsetSunriseProviderMock.Object,
            _sunsetSunriseJsonProcessorMock.Object);
    }
    
    [Test]
    public async Task GetSunsetSunrise_ReturnsBadRequest_IfSunsetSunriseProviderFails()
    {
        _sunsetSunriseProviderMock.Setup(x => x.GetSunsetSunrise(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception());
        
        var result = await _controller.GetSunsetSunrise("city", "date");
        
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
    }
    
    [Test]
    public async Task GetSunsetSunrise_ReturnsOkResult_IfSunsetSunriseProvidersSucceed()
    {
        var sunsetSunriseData = new SunsetSunriseTime();
        _sunsetSunriseProviderMock.Setup(x => x.GetSunsetSunrise(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(sunsetSunriseData.ToString() ?? string.Empty);
        _sunsetSunriseJsonProcessorMock.Setup(x => x.Process(sunsetSunriseData.ToString())).Returns(sunsetSunriseData);
        
        var result = await _controller.GetSunsetSunrise("city", "date");
        
        Assert.IsInstanceOf(typeof(OkObjectResult), result);
    }
}