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
        var expectedSunsetSunriseTime = new SunsetSunriseTime();
        var expectedCityData = new City();
        
        _sunsetSunriseRepositoryMock.Setup(x => x.GetSunsetSunrise(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(expectedSunsetSunriseTime);
        _cityDataRepositoryMock.Setup(x => x.GetCityData(It.IsAny<string>()))
            .ReturnsAsync(expectedCityData);
        
        var result = await _controller.GetSunsetSunrise(It.IsAny<string>(), It.IsAny<string>());
        
        var objectResult = result as OkObjectResult;
        var responseData = objectResult?.Value;
        var responseMessage = GetMessageFromResult(responseData);
        var objectResultData = GetDataFromResult(responseData);
        
        Assert.IsNotNull(responseData);
        Assert.AreEqual("Successfully get the sunset and the sunrise.", responseMessage);
        Assert.IsNotNull(objectResult);
        Assert.IsNotNull(objectResultData);
        Assert.AreEqual(expectedSunsetSunriseTime, objectResultData);
    }

    [Test]
    public async Task GetAllSunsetSunrise_ReturnsBadRequest_IfSunsetSunriseRepositoryThrowsException()
    {
        _sunsetSunriseRepositoryMock.Setup(x => x.GetAllSunsetSunrise()).ThrowsAsync(new Exception());
        
        var result = await _controller.GetAllSunsetSunrise();
        var objectResult = (BadRequestObjectResult?)result;
        var responseData = objectResult?.Value;
        var objectResultMessage = GetMessageFromResult(responseData);
        
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        Assert.That(objectResultMessage, Is.EqualTo("Error getting all sunset and sunrise data"));
    }
    
    [Test]
    public async Task GetAllSunsetSunrise_ReturnsOk_IfSunsetSunriseListIsNotNull()
    {
        var expectedSunsetSunriseTimes = new List<SunsetSunriseTime>
        {
            new SunsetSunriseTime(),
            new SunsetSunriseTime()
        };
        
        _sunsetSunriseRepositoryMock.Setup(x => x.GetAllSunsetSunrise())
            .ReturnsAsync(expectedSunsetSunriseTimes);
        
        var result = await _controller.GetAllSunsetSunrise();
        
        var objectResult = result as OkObjectResult;
        var responseData = objectResult?.Value;
        var responseMessage = GetMessageFromResult(responseData);
        var objectResultData = GetDataFromResult(responseData);
        
        Assert.IsNotNull(responseData);
        Assert.AreEqual("Successfully get all sunset and sunrise.", responseMessage);
        Assert.IsNotNull(objectResult);
        Assert.IsNotNull(objectResultData);
        Assert.AreEqual(expectedSunsetSunriseTimes, objectResultData);
    }

    [Test]
    public async Task AddSunsetSunrise_ReturnsBadRequest_IfSunsetSunriseExists()
    {
        _sunsetSunriseRepositoryMock.Setup(x => x.AddSunsetSunrise(It.IsAny<SunsetSunriseTime>())).ThrowsAsync(new Exception());
        var result = await _controller.AddSunsetSunrise(It.IsAny<SunsetSunriseTime>());
        var objectResult = (BadRequestObjectResult?)result;
        var responseData = objectResult?.Value;
        var objectResultMessage = GetMessageFromResult(responseData);
        
        Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        Assert.That(objectResultMessage, Is.EqualTo("Sunset and sunrise already exists."));
    }
    
    [Test]
    public async Task AddSunsetSunrise_ReturnsOk_IfSunsetSunriseIsNotNull()
    {
        var expectedSunsetSunrise = new SunsetSunriseTime();
        _sunsetSunriseRepositoryMock.Setup(x => x.AddSunsetSunrise(It.IsAny<SunsetSunriseTime>()))
            .ReturnsAsync(expectedSunsetSunrise);
        
        var result = await _controller.AddSunsetSunrise(It.IsAny<SunsetSunriseTime>());
        
        var objectResult = result as OkObjectResult;
        var responseData = objectResult?.Value;
        var responseMessage = GetMessageFromResult(responseData);
        var objectResultData = GetDataFromResult(responseData);
    
        Assert.IsNotNull(responseData);
        Assert.AreEqual("Sunset and sunrise added.", responseMessage);
        Assert.IsNotNull(objectResult);
        Assert.IsNotNull(objectResultData);
        Assert.AreEqual(expectedSunsetSunrise, objectResultData);
    }

    
    [Test]
    public async Task UpdateSunsetSunrise_ReturnsNotFound_IfSunsetSunriseNotFound()
    {
        _sunsetSunriseRepositoryMock.Setup(x => x.UpdateSunsetSunrise(It.IsAny<SunsetSunriseTime>())).ThrowsAsync(new Exception());
        
        var result = await _controller.UpdateSunsetSunrise(It.IsAny<SunsetSunriseTime>());
        var objectResult = (NotFoundObjectResult?)result;
        var responseData = objectResult?.Value;
        var objectResultMessage = GetMessageFromResult(responseData);
        
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result);
        Assert.That(objectResultMessage, Is.EqualTo("Sunset and sunrise not found."));
    }
    
    [Test]
    public async Task UpdateSunsetSunrise_ReturnsOk_IfSunsetSunriseIsNotNull()
    {
        var expectedSunsetSunriseTime = new SunsetSunriseTime();
        
        _sunsetSunriseRepositoryMock.Setup(x => x.UpdateSunsetSunrise(It.IsAny<SunsetSunriseTime>()))
            .ReturnsAsync(expectedSunsetSunriseTime);
        
        var result = await _controller.UpdateSunsetSunrise(It.IsAny<SunsetSunriseTime>());
        
        var objectResult = result as OkObjectResult;
        var responseData = objectResult?.Value;
        var responseMessage = GetMessageFromResult(responseData);
        var objectResultData = GetDataFromResult(responseData);
        
        Assert.IsNotNull(responseData);
        Assert.AreEqual("Sunset and sunrise updated.", responseMessage);
        Assert.IsNotNull(objectResult);
        Assert.IsNotNull(objectResultData);
        Assert.AreEqual(expectedSunsetSunriseTime, objectResultData);
    }
    
    [Test]
    public async Task DeleteSunsetSunrise_ReturnsNotFound_IfSunsetSunriseNotFound()
    {
        _sunsetSunriseRepositoryMock.Setup(x => x.DeleteSunsetSunrise(It.IsAny<int>())).ThrowsAsync(new Exception());
        
        var result = await _controller.DeleteSunsetSunrise(It.IsAny<int>());
        var objectResult = (NotFoundObjectResult?)result;
        var responseData = objectResult?.Value;
        var objectResultMessage = GetMessageFromResult(responseData);
        
        Assert.IsInstanceOf(typeof(NotFoundObjectResult), result);
        Assert.That(objectResultMessage, Is.EqualTo("Sunset and sunrise not found."));
    }
    
    [Test]
    public async Task DeleteSunsetSunrise_ReturnsOk_IfSunsetSunriseFound()
    {
        _sunsetSunriseRepositoryMock.Setup(x => x.DeleteSunsetSunrise(It.IsAny<int>()));
        
        var result = await _controller.DeleteSunsetSunrise(It.IsAny<int>());
        var objectResult = (OkObjectResult?)result;
        var responseData = objectResult?.Value;
        var objectResultMessage = GetMessageFromResult(responseData);
        
        Assert.IsInstanceOf(typeof(OkObjectResult), result);
        Assert.That(objectResultMessage, Is.EqualTo("Sunset and sunrise deleted."));
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