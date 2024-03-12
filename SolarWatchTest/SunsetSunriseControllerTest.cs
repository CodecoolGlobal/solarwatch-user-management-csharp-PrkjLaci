using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Models;
using SolarWatch.Repository.CityRepository;
using SolarWatch.Repository.SunsetSunriseRepository;
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
    private Mock<ICityDataRepository> _cityDataRepositoryMock;
    private Mock<ISunsetSunriseRepository> _sunsetSunriseRepositoryMock;
    private SunsetSunriseController _controller;
    
    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<SunsetSunriseController>>();
        _sunsetSunriseProviderMock = new Mock<ISunsetSunriseApiProvider>();
        _sunsetSunriseJsonProcessorMock = new Mock<ISunsetSunriseJsonProcessor>();
        _geocodingApiProviderMock = new Mock<IGeocodingApiProvider>();
        _cityCoordinatesJsonProcessorMock = new Mock<ICityCoordinatesJsonProcessor>();
        _sunsetSunriseRepositoryMock = new Mock<ISunsetSunriseRepository>();
        _cityDataRepositoryMock = new Mock<ICityDataRepository>();
        _controller = new SunsetSunriseController(_loggerMock.Object, _sunsetSunriseProviderMock.Object,
            _sunsetSunriseJsonProcessorMock.Object, _sunsetSunriseRepositoryMock.Object, _cityDataRepositoryMock.Object,
            _geocodingApiProviderMock.Object, _cityCoordinatesJsonProcessorMock.Object);
    }
    
    [Test]
    public async Task GetSunsetSunrise_ReturnsBadRequest_IfSunsetSunriseRepositoryThrowsException()
    {
        _sunsetSunriseRepositoryMock.Setup(x => x.GetSunsetSunrise(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception());
        
        var result = await _controller.GetSunsetSunrise(It.IsAny<string>(), It.IsAny<string>());
        var objectResult = (BadRequestObjectResult?)result;
        var responseData = objectResult?.Value;
        var objectResultMessage = GetMessageFromResult(responseData);
        
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        Assert.That(objectResultMessage, Is.EqualTo("Error getting sunset and sunrise data"));
    }
    
    [Test]
    public async Task GetSunsetSunrise_ReturnsOk_IfSunsetSunriseIsNotNull()
    {
        _sunsetSunriseRepositoryMock.Setup(x => x.GetSunsetSunrise(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(It.IsAny<SunsetSunriseTime>());
        _cityDataRepositoryMock.Setup(x => x.GetCityData(It.IsAny<string>()))
            .ReturnsAsync(It.IsAny<City>());

        var result = await _controller.GetSunsetSunrise(It.IsAny<string>(), It.IsAny<string>());

        // Assert (Verify the result)
        Assert.That(result, Is.Not.Null);
        Assert.IsInstanceOf(typeof(OkObjectResult), result);
        var objectResult = (OkObjectResult?)result;
        var responseData = objectResult?.Value;
        var objectResultMessage = GetMessageFromResult(responseData);
        var objectResultData = GetDataFromResult(responseData);

        Assert.That(objectResultMessage, Is.EqualTo("Successfully get the sunset and the sunrise."));
        Assert.That(objectResultData, Is.TypeOf<object>()); // Assert data type or properties as needed
    }


    
    private object GetMessageFromResult(object responseData)
    {
        var messageProperty = responseData.GetType().GetProperty("message");
        return messageProperty.GetValue(responseData);
    }
    
    private object GetDataFromResult(object responseData)
    {
        var dataProperty = responseData.GetType().GetProperty("data");
        return dataProperty.GetValue(responseData);
    }
}