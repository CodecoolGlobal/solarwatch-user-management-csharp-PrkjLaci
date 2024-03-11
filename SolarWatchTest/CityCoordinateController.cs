using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using SolarWatch.Controllers;
using SolarWatch.Models;
using SolarWatch.Service.Geocoding;

namespace SolarWatchTest
{
    [TestFixture]
    public class CityCoordinateControllerTests
    {
        private Mock<ILogger<CityCoordinateController>> _loggerMock;
        private Mock<IGeocodingApiProvider> _geocodingApiProviderMock;
        private Mock<ICityCoordinatesJsonProcessor> _cityCoordinatesJsonProcessorMock;
        private CityCoordinateController _controller;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<CityCoordinateController>>();
            _geocodingApiProviderMock = new Mock<IGeocodingApiProvider>();
            _cityCoordinatesJsonProcessorMock = new Mock<ICityCoordinatesJsonProcessor>();
            _controller = new CityCoordinateController(_loggerMock.Object, _geocodingApiProviderMock.Object, _cityCoordinatesJsonProcessorMock.Object);
        }

        [Test]
        public void GetCityCoordinates_ReturnsBadRequest_IfGeocodingApiProviderFails()
        {
            // Arrange
            _geocodingApiProviderMock.Setup(x => x.GetCityCoordinates(It.IsAny<string>())).Throws(new Exception());

            // Act
            var result = _controller.GetCityCoordinates("city");

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }
        
        [Test]
        public void GetCityCoordinates_ReturnsOkResult_IfGeocodingApiProviderSucceeds()
        {
            // Arrange
            var cityCoordinate = new CityCoordinate();
            _geocodingApiProviderMock.Setup(x => x.GetCityCoordinates(It.IsAny<string>())).Returns(cityCoordinate.ToString() ?? string.Empty);
            _cityCoordinatesJsonProcessorMock.Setup(x => x.Process(cityCoordinate.ToString() ?? string.Empty))
                .Returns(cityCoordinate);
            // Act
            var result = _controller.GetCityCoordinates("city");

            // Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), result);
        }
    }
}