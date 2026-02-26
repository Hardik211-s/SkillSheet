using SkillSheetAPI.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SkillSheetAPI.Services.Services
{
    /// <summary>
    /// Service for retrieving weather information from OpenWeatherMap API.
    /// Provides methods to fetch weather data by city name or geographic coordinates.
    /// </summary>
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger<WeatherService> _logger;
        private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";
        private const int HttpTimeoutSeconds = 10;

        // JSON property name constants to avoid magic strings
        private static class JsonProperties
        {
            public const string CityName = "name";
            public const string System = "sys";
            public const string Country = "country";
            public const string Main = "main";
            public const string Temperature = "temp";
            public const string FeelsLike = "feels_like";
            public const string Humidity = "humidity";
            public const string Pressure = "pressure";
            public const string Wind = "wind";
            public const string WindSpeed = "speed";
            public const string Clouds = "clouds";
            public const string CloudsAll = "all";
            public const string Weather = "weather";
            public const string Description = "description";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client for making API requests.</param>
        /// <param name="apiKey">The OpenWeatherMap API key. Should be stored securely in configuration.</param>
        /// <param name="logger">Logger for diagnostic information.</param>
        /// <remarks>
        /// SECURITY NOTE: API keys should NEVER be hardcoded. Always use secure configuration methods:
        /// - Azure Key Vault for cloud deployments
        /// - Environment variables for local/docker development
        /// - User Secrets for development machines
        /// Never commit API keys to source control.
        /// </remarks>
        public WeatherService(HttpClient httpClient, string apiKey, ILogger<WeatherService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            
            if (_httpClient.Timeout < TimeSpan.FromSeconds(HttpTimeoutSeconds))
            {
                _httpClient.Timeout = TimeSpan.FromSeconds(HttpTimeoutSeconds);
            }
        }

        /// <summary>
        /// Gets weather information for a specific city.
        /// </summary>
        /// <param name="city">The name of the city (e.g., "London", "New York").</param>
        /// <returns>A task that represents the asynchronous operation and contains weather data.</returns>
        /// <exception cref="ArgumentNullException">Thrown when city parameter is null or empty.</exception>
        /// <exception cref="HttpRequestException">Thrown when HTTP request fails.</exception>
        /// <exception cref="InvalidOperationException">Thrown when API response parsing fails.</exception>
        public async Task<WeatherData> GetWeatherByCityAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                _logger.LogWarning("GetWeatherByCityAsync called with empty city parameter");
                throw new ArgumentNullException(nameof(city), "City name cannot be null or empty");
            }

            try
            {
                _logger.LogInformation("Fetching weather data for city: {City}", city);
                
                var url = $"{BaseUrl}?q={Uri.EscapeDataString(city)}&units=metric&appid={_apiKey}";
                using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(HttpTimeoutSeconds));
                var response = await _httpClient.GetAsync(url, cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to retrieve weather for city {City}. Status: {StatusCode}", city, response.StatusCode);
                    throw new HttpRequestException($"OpenWeatherMap API returned status code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                
                if (string.IsNullOrEmpty(content))
                {
                    _logger.LogError("Empty response received from OpenWeatherMap API for city: {City}", city);
                    throw new InvalidOperationException("Empty response from weather API");
                }

                var weatherData = ParseWeatherResponse(content);
                _logger.LogInformation("Successfully retrieved weather data for city: {City}", city);
                return weatherData;
            }
            catch (System.Threading.Tasks.TaskCanceledException)
            {
                _logger.LogError("Request timeout while fetching weather for city: {City}", city);
                throw new HttpRequestException($"Request timeout while fetching weather for {city}");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse weather API response for city: {City}", city);
                throw new InvalidOperationException($"Invalid response format from weather API for city {city}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching weather for city: {City}", city);
                throw;
            }
        }

        /// <summary>
        /// Gets weather information for coordinates (latitude and longitude).
        /// </summary>
        /// <param name="latitude">The latitude coordinate (-90 to 90).</param>
        /// <param name="longitude">The longitude coordinate (-180 to 180).</param>
        /// <returns>A task that represents the asynchronous operation and contains weather data.</returns>
        /// <exception cref="ArgumentException">Thrown when coordinates are invalid.</exception>
        /// <exception cref="HttpRequestException">Thrown when HTTP request fails.</exception>
        /// <exception cref="InvalidOperationException">Thrown when API response parsing fails.</exception>
        public async Task<WeatherData> GetWeatherByCoordinatesAsync(double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90)
            {
                _logger.LogWarning("GetWeatherByCoordinatesAsync called with invalid latitude: {Latitude}", latitude);
                throw new ArgumentException("Latitude must be between -90 and 90", nameof(latitude));
            }

            if (longitude < -180 || longitude > 180)
            {
                _logger.LogWarning("GetWeatherByCoordinatesAsync called with invalid longitude: {Longitude}", longitude);
                throw new ArgumentException("Longitude must be between -180 and 180", nameof(longitude));
            }

            try
            {
                _logger.LogInformation("Fetching weather data for coordinates: Latitude={Latitude}, Longitude={Longitude}", latitude, longitude);
                
                var url = $"{BaseUrl}?lat={latitude}&lon={longitude}&units=metric&appid={_apiKey}";
                using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(HttpTimeoutSeconds));
                var response = await _httpClient.GetAsync(url, cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to retrieve weather for coordinates ({Latitude}, {Longitude}). Status: {StatusCode}", latitude, longitude, response.StatusCode);
                    throw new HttpRequestException($"OpenWeatherMap API returned status code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                
                if (string.IsNullOrEmpty(content))
                {
                    _logger.LogError("Empty response received from OpenWeatherMap API for coordinates: ({Latitude}, {Longitude})", latitude, longitude);
                    throw new InvalidOperationException("Empty response from weather API");
                }

                var weatherData = ParseWeatherResponse(content);
                _logger.LogInformation("Successfully retrieved weather data for coordinates: ({Latitude}, {Longitude})", latitude, longitude);
                return weatherData;
            }
            catch (System.Threading.Tasks.TaskCanceledException)
            {
                _logger.LogError("Request timeout while fetching weather for coordinates: ({Latitude}, {Longitude})", latitude, longitude);
                throw new HttpRequestException($"Request timeout while fetching weather for coordinates");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse weather API response for coordinates: ({Latitude}, {Longitude})", latitude, longitude);
                throw new InvalidOperationException($"Invalid response format from weather API", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching weather for coordinates: ({Latitude}, {Longitude})", latitude, longitude);
                throw;
            }
        }

        /// <summary>
        /// Parses the weather API response JSON and extracts weather data.
        /// Uses safe property access to handle missing or unexpected fields gracefully.
        /// </summary>
        /// <param name="jsonContent">The JSON response content from the OpenWeatherMap API.</param>
        /// <returns>A <see cref="WeatherData"/> object containing parsed weather information.</returns>
        /// <exception cref="InvalidOperationException">Thrown when required fields are missing from the response.</exception>
        /// <exception cref="JsonException">Thrown when JSON is invalid.</exception>
        private WeatherData ParseWeatherResponse(string jsonContent)
        {
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(jsonContent))
                {
                    var root = doc.RootElement;

                    // Safely extract all properties with fallback values
                    var city = SafeGetStringProperty(root, JsonProperties.CityName, "Unknown");
                    var country = SafeGetStringProperty(root, $"{JsonProperties.System}.{JsonProperties.Country}", "Unknown");
                    var temperature = SafeGetDoubleProperty(root, $"{JsonProperties.Main}.{JsonProperties.Temperature}", 0);
                    var feelsLike = SafeGetDoubleProperty(root, $"{JsonProperties.Main}.{JsonProperties.FeelsLike}", 0);
                    var description = SafeGetStringProperty(root, $"{JsonProperties.Weather}[0].{JsonProperties.Description}", "Unknown");
                    var humidity = SafeGetIntProperty(root, $"{JsonProperties.Main}.{JsonProperties.Humidity}", 0);
                    var windSpeed = SafeGetDoubleProperty(root, $"{JsonProperties.Wind}.{JsonProperties.WindSpeed}", 0);
                    var pressure = SafeGetIntProperty(root, $"{JsonProperties.Main}.{JsonProperties.Pressure}", 0);
                    var clouds = SafeGetIntProperty(root, $"{JsonProperties.Clouds}.{JsonProperties.CloudsAll}", 0);

                    // Validate that we at least got the city name
                    if (city == "Unknown")
                    {
                        throw new InvalidOperationException("City name not found in API response");
                    }

                    var weatherData = new WeatherData
                    {
                        City = city,
                        Country = country,
                        Temperature = temperature,
                        FeelsLike = feelsLike,
                        Description = description,
                        Humidity = humidity,
                        WindSpeed = windSpeed,
                        Pressure = pressure,
                        Clouds = clouds,
                        Timestamp = DateTime.UtcNow
                    };

                    return weatherData;
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing error while parsing weather response");
                throw new InvalidOperationException("Invalid JSON response from weather API", ex);
            }
        }

        /// <summary>
        /// Safely extracts a string property from JSON, returning a default value if not found.
        /// </summary>
        private string SafeGetStringProperty(JsonElement element, string propertyPath, string defaultValue)
        {
            try
            {
                var parts = propertyPath.Split('.');
                JsonElement current = element;

                foreach (var part in parts)
                {
                    if (part.Contains("["))
                    {
                        // Handle array access like "weather[0]"
                        var arrayName = part.Substring(0, part.IndexOf('['));
                        var index = int.Parse(part.Substring(part.IndexOf('[') + 1, part.IndexOf(']') - part.IndexOf('[') - 1));

                        if (!current.TryGetProperty(arrayName, out var arrayElement))
                            return defaultValue;

                        current = arrayElement[index];
                    }
                    else
                    {
                        if (!current.TryGetProperty(part, out var nextElement))
                            return defaultValue;

                        current = nextElement;
                    }
                }

                return current.GetString() ?? defaultValue;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Safely extracts a double property from JSON, returning a default value if not found.
        /// </summary>
        private double SafeGetDoubleProperty(JsonElement element, string propertyPath, double defaultValue)
        {
            try
            {
                var parts = propertyPath.Split('.');
                JsonElement current = element;

                foreach (var part in parts)
                {
                    if (!current.TryGetProperty(part, out var nextElement))
                        return defaultValue;

                    current = nextElement;
                }

                return current.GetDouble();
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Safely extracts an integer property from JSON, returning a default value if not found.
        /// </summary>
        private int SafeGetIntProperty(JsonElement element, string propertyPath, int defaultValue)
        {
            try
            {
                var parts = propertyPath.Split('.');
                JsonElement current = element;

                foreach (var part in parts)
                {
                    if (!current.TryGetProperty(part, out var nextElement))
                        return defaultValue;

                    current = nextElement;
                }

                return current.GetInt32();
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}
