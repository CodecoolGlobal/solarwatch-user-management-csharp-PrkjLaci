using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
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
    public void GetSunsetSunriseReturnsBadRequestIfGeocodingApiProviderFails()
    {
        _geocodingApiProviderMock.Setup(x => x.GetCityCoordinates(It.IsAny<string>())).Throws(new Exception());
        
        var result = _controller.GetSunsetSunrise("city", "date");
        
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
    }
    
    [Test]
    public void GetSunsetSunriseReturnsBadRequestIfCityCoordinatesJsonProcessorFails()
    {
        var cityData = "{}";
        _geocodingApiProviderMock.Setup(x => x.GetCityCoordinates(It.IsAny<string>())).Returns(cityData);
        _cityCoordinatesJsonProcessorMock.Setup(x => x.Process(cityData)).Throws<Exception>();
        
        var result = _controller.GetSunsetSunrise("city", "date");
        
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
    }
}