using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SkillSheetAPI.Services.Interfaces;
using SkillSheetAPI.Services.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SkillSheet.Tests.Services
{
    /// <summary>
    /// Unit tests for the WeatherService class.
    /// </summary>
    [TestClass]
    public class WeatherServiceTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpClient _httpClient;
        private Mock<ILogger<WeatherService>> _loggerMock;
        private WeatherService _weatherService;
        private const string TestApiKey = "test-api-key";

        [TestInitialize]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _loggerMock = new Mock<ILogger<WeatherService>>();
            _weatherService = new WeatherService(_httpClient, TestApiKey, _loggerMock.Object);
        }

        #region GetWeatherByCityAsync Tests

        [TestMethod]
        public async Task GetWeatherByCityAsync_WithValidCity_ReturnsWeatherData()
        {
            // Arrange
            var city = "London";
            var validResponse = @"{
                ""name"": ""London"",
                ""sys"": { ""country"": ""GB"" },
                ""main"": { 
                    ""temp"": 15.5, 
                    ""feels_like"": 14.8, 
                    ""humidity"": 65, 
                    ""pressure"": 1013 
                },
                ""weather"": [{ ""description"": ""partly cloudy"" }],
                ""wind"": { ""speed"": 3.5 },
                ""clouds"": { ""all"": 40 }
            }";

            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(validResponse)
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _weatherService.GetWeatherByCityAsync(city);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("London", result.City);
            Assert.AreEqual("GB", result.Country);
            Assert.AreEqual(15.5, result.Temperature);
            Assert.AreEqual(65, result.Humidity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetWeatherByCityAsync_WithNullCity_ThrowsArgumentNullException()
        {
            // Act
            await _weatherService.GetWeatherByCityAsync(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetWeatherByCityAsync_WithEmptyCity_ThrowsArgumentNullException()
        {
            // Act
            await _weatherService.GetWeatherByCityAsync(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task GetWeatherByCityAsync_WithFailedApiCall_ThrowsHttpRequestException()
        {
            // Arrange
            var city = "InvalidCity";
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(mockResponse);

            // Act
            await _weatherService.GetWeatherByCityAsync(city);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetWeatherByCityAsync_WithEmptyResponse_ThrowsInvalidOperationException()
        {
            // Arrange
            var city = "London";
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(string.Empty)
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(mockResponse);

            // Act
            await _weatherService.GetWeatherByCityAsync(city);
        }

        [TestMethod]
        public async Task GetWeatherByCityAsync_WithMissingOptionalFields_ReturnsDataWithDefaults()
        {
            // Arrange
            var city = "London";
            var partialResponse = @"{
                ""name"": ""London"",
                ""sys"": { ""country"": ""GB"" },
                ""main"": { ""temp"": 15.5 },
                ""weather"": [{ ""description"": ""cloudy"" }],
                ""wind"": { ""speed"": 3.5 },
                ""clouds"": { ""all"": 40 }
            }";

            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(partialResponse)
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _weatherService.GetWeatherByCityAsync(city);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("London", result.City);
            // Missing fields should have default values
            Assert.AreEqual(0, result.Humidity);
        }

        #endregion

        #region GetWeatherByCoordinatesAsync Tests

        [TestMethod]
        public async Task GetWeatherByCoordinatesAsync_WithValidCoordinates_ReturnsWeatherData()
        {
            // Arrange
            var latitude = 51.5074;
            var longitude = -0.1278;
            var validResponse = @"{
                ""name"": ""London"",
                ""sys"": { ""country"": ""GB"" },
                ""main"": { 
                    ""temp"": 15.5, 
                    ""feels_like"": 14.8, 
                    ""humidity"": 65, 
                    ""pressure"": 1013 
                },
                ""weather"": [{ ""description"": ""partly cloudy"" }],
                ""wind"": { ""speed"": 3.5 },
                ""clouds"": { ""all"": 40 }
            }";

            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(validResponse)
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _weatherService.GetWeatherByCoordinatesAsync(latitude, longitude);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("London", result.City);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetWeatherByCoordinatesAsync_WithInvalidLatitude_ThrowsArgumentException()
        {
            // Act
            await _weatherService.GetWeatherByCoordinatesAsync(91, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task GetWeatherByCoordinatesAsync_WithInvalidLongitude_ThrowsArgumentException()
        {
            // Act
            await _weatherService.GetWeatherByCoordinatesAsync(0, 181);
        }

        [TestMethod]
        public async Task GetWeatherByCoordinatesAsync_WithBoundaryLatitude_Succeeds()
        {
            // Arrange
            var latitude = 90; // Boundary value
            var longitude = 0;
            var validResponse = @"{
                ""name"": ""NorthPole"",
                ""sys"": { ""country"": ""XX"" },
                ""main"": { ""temp"": -40 },
                ""weather"": [{ ""description"": ""snowy"" }],
                ""wind"": { ""speed"": 10 },
                ""clouds"": { ""all"": 100 }
            }";

            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(validResponse)
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(mockResponse);

            // Act
            var result = await _weatherService.GetWeatherByCoordinatesAsync(latitude, longitude);

            // Assert
            Assert.IsNotNull(result);
        }

        #endregion

        #region Error Handling Tests

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task GetWeatherByCityAsync_WithInvalidJson_ThrowsInvalidOperationException()
        {
            // Arrange
            var city = "London";
            var invalidResponse = "{ invalid json }";

            var mockResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(invalidResponse)
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(mockResponse);

            // Act
            await _weatherService.GetWeatherByCityAsync(city);
        }

        [TestMethod]
        public void Constructor_WithNullHttpClient_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(
                () => new WeatherService(null, TestApiKey, _loggerMock.Object)
            );
        }

        [TestMethod]
        public void Constructor_WithNullApiKey_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(
                () => new WeatherService(_httpClient, null, _loggerMock.Object)
            );
        }

        [TestMethod]
        public void Constructor_WithNullLogger_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(
                () => new WeatherService(_httpClient, TestApiKey, null)
            );
        }

        #endregion
    }
}
