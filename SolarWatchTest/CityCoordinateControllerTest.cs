using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Models;
using SolarWatch.Repository.CityRepository;
using SolarWatch.Service.Geocoding;

namespace SolarWatchTest;

    [TestFixture]
    public class CityCoordinateControllerTests
    {
        private Mock<ILogger<CityDataController>> _loggerMock;
        private Mock<ICityDataRepository> _cityDataRepositoryMock;
        private Mock<IGeocodingApiProvider> _geocodingApiProviderMock;
        private Mock<ICityCoordinatesJsonProcessor> _cityCoordinatesJsonProcessorMock;
        private CityDataController _controller;
        

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<CityDataController>>();
            _cityDataRepositoryMock = new Mock<ICityDataRepository>();
            _geocodingApiProviderMock = new Mock<IGeocodingApiProvider>();
            _cityCoordinatesJsonProcessorMock = new Mock<ICityCoordinatesJsonProcessor>();
            _controller = new CityDataController(_loggerMock.Object, _geocodingApiProviderMock.Object, _cityCoordinatesJsonProcessorMock.Object, _cityDataRepositoryMock.Object);
            
        }

        [Test]
        public async Task GetCityCoordinates_ReturnsBadRequest_IfCityDataRepositoryThrowsException()
        {
            _cityDataRepositoryMock.Setup(x => x.GetCityData(It.IsAny<string>())).ThrowsAsync(new Exception());
            
            var result = await _controller.GetCityCoordinates(It.IsAny<string>());
            var objectResult = (BadRequestObjectResult?)result;
            var responseData = objectResult?.Value;
            var objectResultMessage = GetMessageFromResult(responseData);
            
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
            Assert.That(objectResultMessage, Is.EqualTo("Error getting city coordinates"));
        }
        
        [Test]
        public async Task GetCityCoordinates_ReturnsOk_IfCityDataIsNotNull()
        {
            _cityDataRepositoryMock.Setup(x => x.GetCityData(It.IsAny<string>())).ReturnsAsync(It.IsAny<City>());
            
            var result = await _controller.GetCityCoordinates(It.IsAny<string>());
            var objectResult = (OkObjectResult?)result;
            var responseData = objectResult?.Value;
            var objectResultMessage = GetMessageFromResult(responseData);
            var objectResultData = GetDataFromResult(responseData);
            
            Assert.IsInstanceOf(typeof(OkObjectResult), result);
            Assert.That(objectResultMessage, Is.EqualTo("Successfully get city data."));
            Assert.That(objectResultData, Is.EqualTo(It.IsAny<City>()));
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