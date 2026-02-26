using Microsoft.AspNetCore.Mvc;
using SkillSheetAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace SkillSheetAPI.Controllers
{
    /// <summary>
    /// Controller for weather-related API endpoints.
    /// Provides REST endpoints to retrieve weather information by city name or geographic coordinates.
    /// </summary>
    [ApiController]
    [Route("api/weather")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherController"/> class.
        /// </summary>
        /// <param name="weatherService">The weather service for retrieving weather data.</param>
        /// <param name="logger">Logger for diagnostic information.</param>
        /// <exception cref="ArgumentNullException">Thrown when dependencies are null.</exception>
        public WeatherController(IWeatherService weatherService, ILogger<WeatherController> logger)
        {
            _weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets weather information for a specific city.
        /// </summary>
        /// <param name="city">The name of the city (required).</param>
        /// <returns>
        /// 200 OK with weather data if successful.
        /// 400 Bad Request if city parameter is missing or API fails.
        /// 500 Internal Server Error if an unexpected error occurs.
        /// </returns>
        /// <remarks>
        /// Example request: GET /api/weather/city?city=London
        /// 
        /// Example response:
        /// {
        ///   "message": "Weather data retrieved successfully.",
        ///   "data": {
        ///     "city": "London",
        ///     "country": "GB",
        ///     "temperature": 15.5,
        ///     "feelsLike": 14.8,
        ///     "description": "partly cloudy",
        ///     "humidity": 65,
        ///     "windSpeed": 3.5,
        ///     "pressure": 1013,
        ///     "clouds": 40,
        ///     "timestamp": "2026-02-26T05:45:00Z"
        ///   }
        /// }
        /// </remarks>
        [HttpGet("city")]
        [Produces("application/json")]
        [ProduceResponseType(typeof(WeatherResponseDto), 200)]
        [ProduceResponseType(typeof(ErrorResponseDto), 400)]
        [ProduceResponseType(typeof(ErrorResponseDto), 500)]
        public async Task<IActionResult> GetWeatherByCity([FromQuery] string city)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(city))
                {
                    _logger.LogWarning("GetWeatherByCity called with empty city parameter");
                    return BadRequest(new ErrorResponseDto 
                    { 
                        Message = "City name is required and cannot be empty.",
                        ErrorCode = "MISSING_PARAMETER",
                        Timestamp = DateTime.UtcNow
                    });
                }

                if (city.Length > 100)
                {
                    _logger.LogWarning("GetWeatherByCity called with city name exceeding maximum length: {City}", city);
                    return BadRequest(new ErrorResponseDto 
                    { 
                        Message = "City name cannot exceed 100 characters.",
                        ErrorCode = "INVALID_PARAMETER",
                        Timestamp = DateTime.UtcNow
                    });
                }

                _logger.LogInformation("Processing request to get weather for city: {City}", city);
                var weather = await _weatherService.GetWeatherByCityAsync(city);
                
                return Ok(new WeatherResponseDto
                {
                    Message = "Weather data retrieved successfully.",
                    Data = weather,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogWarning(ex, "Invalid argument in GetWeatherByCity: {City}", city);
                return BadRequest(new ErrorResponseDto 
                { 
                    Message = "Invalid input parameter.",
                    ErrorCode = "INVALID_PARAMETER",
                    Details = ex.Message,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error while fetching weather for city: {City}", city);
                return BadRequest(new ErrorResponseDto 
                { 
                    Message = "Failed to retrieve weather data from the weather service.",
                    ErrorCode = "EXTERNAL_API_ERROR",
                    Details = ex.Message,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation while fetching weather for city: {City}", city);
                return BadRequest(new ErrorResponseDto 
                { 
                    Message = "Invalid response received from weather service.",
                    ErrorCode = "PARSING_ERROR",
                    Details = ex.Message,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetWeatherByCity for city: {City}", city);
                return StatusCode(500, new ErrorResponseDto 
                { 
                    Message = "An unexpected error occurred while processing your request.",
                    ErrorCode = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Gets weather information for specific geographic coordinates.
        /// </summary>
        /// <param name="latitude">The latitude coordinate (-90 to 90, required).</param>
        /// <param name="longitude">The longitude coordinate (-180 to 180, required).</param>
        /// <returns>
        /// 200 OK with weather data if successful.
        /// 400 Bad Request if coordinates are invalid or API fails.
        /// 500 Internal Server Error if an unexpected error occurs.
        /// </returns>
        /// <remarks>
        /// Example request: GET /api/weather/coordinates?latitude=51.5074&longitude=-0.1278
        /// 
        /// Valid coordinate ranges:
        /// - Latitude: -90 to 90
        /// - Longitude: -180 to 180
        /// </remarks>
        [HttpGet("coordinates")]
        [Produces("application/json")]
        [ProduceResponseType(typeof(WeatherResponseDto), 200)]
        [ProduceResponseType(typeof(ErrorResponseDto), 400)]
        [ProduceResponseType(typeof(ErrorResponseDto), 500)]
        public async Task<IActionResult> GetWeatherByCoordinates([FromQuery] double latitude, [FromQuery] double longitude)
        {
            try
            {
                // Validate latitude
                if (latitude < -90 || latitude > 90)
                {
                    _logger.LogWarning("GetWeatherByCoordinates called with invalid latitude: {Latitude}", latitude);
                    return BadRequest(new ErrorResponseDto 
                    { 
                        Message = "Latitude must be between -90 and 90.",
                        ErrorCode = "INVALID_LATITUDE",
                        Details = $"Received latitude: {latitude}",
                        Timestamp = DateTime.UtcNow
                    });
                }

                // Validate longitude
                if (longitude < -180 || longitude > 180)
                {
                    _logger.LogWarning("GetWeatherByCoordinates called with invalid longitude: {Longitude}", longitude);
                    return BadRequest(new ErrorResponseDto 
                    { 
                        Message = "Longitude must be between -180 and 180.",
                        ErrorCode = "INVALID_LONGITUDE",
                        Details = $"Received longitude: {longitude}",
                        Timestamp = DateTime.UtcNow
                    });
                }

                _logger.LogInformation("Processing request to get weather for coordinates: Latitude={Latitude}, Longitude={Longitude}", latitude, longitude);
                var weather = await _weatherService.GetWeatherByCoordinatesAsync(latitude, longitude);
                
                return Ok(new WeatherResponseDto
                {
                    Message = "Weather data retrieved successfully.",
                    Data = weather,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid coordinate argument in GetWeatherByCoordinates: Latitude={Latitude}, Longitude={Longitude}", latitude, longitude);
                return BadRequest(new ErrorResponseDto 
                { 
                    Message = "Invalid coordinate values provided.",
                    ErrorCode = "INVALID_COORDINATES",
                    Details = ex.Message,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error while fetching weather for coordinates: Latitude={Latitude}, Longitude={Longitude}", latitude, longitude);
                return BadRequest(new ErrorResponseDto 
                { 
                    Message = "Failed to retrieve weather data from the weather service.",
                    ErrorCode = "EXTERNAL_API_ERROR",
                    Details = ex.Message,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid operation while fetching weather for coordinates: Latitude={Latitude}, Longitude={Longitude}", latitude, longitude);
                return BadRequest(new ErrorResponseDto 
                { 
                    Message = "Invalid response received from weather service.",
                    ErrorCode = "PARSING_ERROR",
                    Details = ex.Message,
                    Timestamp = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetWeatherByCoordinates for coordinates: Latitude={Latitude}, Longitude={Longitude}", latitude, longitude);
                return StatusCode(500, new ErrorResponseDto 
                { 
                    Message = "An unexpected error occurred while processing your request.",
                    ErrorCode = "INTERNAL_SERVER_ERROR",
                    Details = ex.Message,
                    Timestamp = DateTime.UtcNow
                });
            }
        }
    }

    /// <summary>
    /// DTO for successful weather API responses.
    /// </summary>
    public class WeatherResponseDto
    {
        /// <summary>
        /// Gets or sets the response message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the weather data.
        /// </summary>
        public WeatherData Data { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the response.
        /// </summary>
        public DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// DTO for error responses from the weather API.
    /// </summary>
    public class ErrorResponseDto
    {
        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the error code for programmatic handling.
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets additional error details (optional).
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the error.
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
