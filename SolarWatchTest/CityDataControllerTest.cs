using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
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
        public async Task GetAllCities_ReturnsBadRequest_IfCityDataRepositoryThrowsException()
        {
            _cityDataRepositoryMock.Setup(x => x.GetAllCityData()).ThrowsAsync(new Exception());
            
            var result = await _controller.GetAllCityData();
            Console.WriteLine(result);
            var objectResult = (BadRequestObjectResult?)result;
            var responseData = objectResult?.Value;
            var objectResultMessage = GetMessageFromResult(responseData);
            
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
            Assert.That(objectResultMessage, Is.EqualTo("Error getting city data"));
        }
        
        [Test]
        public async Task GetAllCities_ReturnsOk_IfCityDataIsNotNull()
        {
            _cityDataRepositoryMock.Setup(x => x.GetAllCityData()).ReturnsAsync(It.IsAny<List<City>>());
            
            var result = await _controller.GetAllCityData();
            var objectResult = (OkObjectResult?)result;
            var responseData = objectResult?.Value;
            var objectResultMessage = GetMessageFromResult(responseData);
            var objectResultData = GetDataFromResult(responseData);
            
            Assert.IsInstanceOf(typeof(OkObjectResult), result);
            Assert.That(objectResultMessage, Is.EqualTo("Successfully get all city data."));
            Assert.That(objectResultData, Is.EqualTo(It.IsAny<List<City>>()));
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
        
        [Test]
        public async Task GetCityDataById_ReturnsNotFound_IfCityDataIsNull()
        {
            _cityDataRepositoryMock.Setup(x => x.GetCityDataById(It.IsAny<int>())).ReturnsAsync((City)null);
            
            var result = await _controller.GetCityDataById(It.IsAny<int>());
            var objectResult = (NotFoundObjectResult?)result;
            var responseData = objectResult?.Value;
            var objectResultMessage = GetMessageFromResult(responseData);
            
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), result);
            Assert.That(objectResultMessage, Is.EqualTo("City data not found."));
        }
        
        [Test]
        public async Task GetCityDataById_ReturnsOk_IfCityDataIsNotNull()
        {
            var expectedCity = new City { Id = 1, CityName = "Budapest", Latitude = 47.4979, Longitude = 19.0402, Country = "Hungary" };
            _cityDataRepositoryMock.Setup(x => x.GetCityDataById(It.IsAny<int>())).ReturnsAsync(expectedCity);
            var result = await _controller.GetCityDataById(It.IsAny<int>());
            var objectResult = (OkObjectResult)result;
            var responseData = objectResult.Value;
            var objectResultMessage = GetMessageFromResult(responseData);
            var objectResultData = GetDataFromResult(responseData);
            
            Assert.IsInstanceOf(typeof(OkObjectResult), result);
            Assert.That(objectResultMessage, Is.EqualTo("City data found."));
            Assert.That(objectResultData, Is.EqualTo(expectedCity));
        }
        
        [Test]
        public async Task AddCityData_ReturnsBadRequest_IfCityDataExists()
        {
            _cityDataRepositoryMock.Setup(x => x.SaveCityData(It.IsAny<City>())).ThrowsAsync(new Exception());
            var result = await _controller.AddCityData(It.IsAny<City>());
            var objectResult = (BadRequestObjectResult)result;
            var responseData = objectResult.Value;
            var objectResultMessage = GetMessageFromResult(responseData);
            
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
            Assert.That(objectResultMessage, Is.EqualTo("City data already exists."));
        }
        
        [Test]
        public async Task AddCityData_ReturnsOk_IfCityDataIsNotNull()
        {
            _cityDataRepositoryMock.Setup(x => x.AddCityData(It.IsAny<City>()));
            var result = await _controller.AddCityData(It.IsAny<City>());
            var objectResult = (OkObjectResult)result;
            var responseData = objectResult.Value;
            var objectResultMessage = GetMessageFromResult(responseData);
            var objectResultData = GetDataFromResult(responseData);
            
            Assert.IsInstanceOf(typeof(OkObjectResult), result);
            Assert.That(objectResultMessage, Is.EqualTo("City data added."));
            Assert.That(objectResultData, Is.EqualTo(It.IsAny<City>()));
        }
        
        [Test]
        public async Task UpdateCityData_ReturnsNotFound_IfCityDataNotFound()
        {
            _cityDataRepositoryMock.Setup(x => x.UpdateCityData(It.IsAny<City>())).ThrowsAsync(new Exception());
            var result = await _controller.UpdateCityData(It.IsAny<City>());
            var objectResult = (NotFoundObjectResult)result;
            var responseData = objectResult.Value;
            var objectResultMessage = GetMessageFromResult(responseData);
            
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), result);
            Assert.AreEqual("City data not found.", objectResultMessage);
        }
        
        [Test]
        public async Task UpdateCityData_ReturnsOk_IfCityDataFound()
        {
            var expectedCity = new City { Id = 1, CityName = "Budapest", Latitude = 47.4979, Longitude = 19.0402, Country = "Hungary" };
            _cityDataRepositoryMock.Setup(x => x.UpdateCityData(It.IsAny<City>())).ReturnsAsync(expectedCity);
            var result = await _controller.UpdateCityData(It.IsAny<City>());
            var objectResult = (OkObjectResult)result;
            var responseData = objectResult.Value;
            var objectResultMessage = GetMessageFromResult(responseData);
            var objectResultData = GetDataFromResult(responseData);
            
            Assert.IsInstanceOf(typeof(OkObjectResult), result);
            Assert.AreEqual("City data updated.", objectResultMessage);
            Assert.AreEqual(expectedCity, objectResultData);
        }
        
        [Test]
        public async Task DeleteCityData_ReturnsNotFound_IfCityDataNotFound()
        {
            _cityDataRepositoryMock.Setup(x => x.DeleteCityData(It.IsAny<int>())).ThrowsAsync(new Exception());
            var result = await _controller.DeleteCityData(It.IsAny<int>());
            var objectResult = (NotFoundObjectResult)result;
            var responseData = objectResult.Value;
            var objectResultMessage = GetMessageFromResult(responseData);
            
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), result);
            Assert.AreEqual("City data not found.", objectResultMessage);
        }
        
        [Test]
        public async Task DeleteCityData_ReturnsOk_IfCityDataFound()
        {
            _cityDataRepositoryMock.Setup(x => x.DeleteCityData(It.IsAny<int>()));
            var result = await _controller.DeleteCityData(It.IsAny<int>());
            var objectResult = (OkObjectResult)result;
            var responseData = objectResult.Value;
            var objectResultMessage = GetMessageFromResult(responseData);
            
            Assert.IsInstanceOf(typeof(OkObjectResult), result);
            Assert.AreEqual("City data deleted.", objectResultMessage);
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