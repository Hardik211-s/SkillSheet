using SkillSheetAPI.Services.Interfaces;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace SkillSheetAPI.Services.Services
{
    /// <summary>
    /// Service for retrieving weather information from OpenWeatherMap API.
    /// </summary>
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherService"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client for making API requests.</param>
        /// <param name="apiKey">The OpenWeatherMap API key.</param>
        public WeatherService(HttpClient httpClient, string apiKey)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
        }

        /// <summary>
        /// Gets weather information for a specific city.
        /// </summary>
        /// <param name="city">The name of the city.</param>
        /// <returns>A task that represents the asynchronous operation and contains weather data.</returns>
        public async Task<WeatherData> GetWeatherByCityAsync(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentException("City name cannot be null or empty", nameof(city));
            }

            try
            {
                var url = $"{BaseUrl}?q={Uri.EscapeDataString(city)}&units=metric&appid={_apiKey}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"OpenWeatherMap API returned status code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                
                if (string.IsNullOrEmpty(content))
                {
                    throw new InvalidOperationException("Empty response from weather API");
                }

                return ParseWeatherResponse(content);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error fetching weather for city {city}: {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Error parsing weather response for city {city}: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error fetching weather for city {city}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets weather information for coordinates (latitude and longitude).
        /// </summary>
        /// <param name="latitude">The latitude coordinate.</param>
        /// <param name="longitude">The longitude coordinate.</param>
        /// <returns>A task that represents the asynchronous operation and contains weather data.</returns>
        public async Task<WeatherData> GetWeatherByCoordinatesAsync(double latitude, double longitude)
        {
            if (latitude < -90 || latitude > 90)
            {
                throw new ArgumentException("Latitude must be between -90 and 90", nameof(latitude));
            }

            if (longitude < -180 || longitude > 180)
            {
                throw new ArgumentException("Longitude must be between -180 and 180", nameof(longitude));
            }

            try
            {
                var url = $"{BaseUrl}?lat={latitude}&lon={longitude}&units=metric&appid={_apiKey}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"OpenWeatherMap API returned status code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                
                if (string.IsNullOrEmpty(content))
                {
                    throw new InvalidOperationException("Empty response from weather API");
                }

                return ParseWeatherResponse(content);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error fetching weather for coordinates ({latitude}, {longitude}): {ex.Message}", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Error parsing weather response for coordinates ({latitude}, {longitude}): {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error fetching weather for coordinates ({latitude}, {longitude}): {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Parses the weather API response using safe JSON property access.
        /// </summary>
        /// <param name="jsonContent">The JSON response content from the API.</param>
        /// <returns>A <see cref="WeatherData"/> object containing parsed weather information.</returns>
        private WeatherData ParseWeatherResponse(string jsonContent)
        {
            try
            {
                using (JsonDocument doc = JsonDocument.Parse(jsonContent))
                {
                    var root = doc.RootElement;

                    // Safe property extraction with defaults
                    string city = "Unknown";
                    if (root.TryGetProperty("name", out var nameElement))
                    {
                        city = nameElement.GetString() ?? "Unknown";
                    }

                    string country = "Unknown";
                    if (root.TryGetProperty("sys", out var sysElement) && 
                        sysElement.TryGetProperty("country", out var countryElement))
                    {
                        country = countryElement.GetString() ?? "Unknown";
                    }

                    double temperature = 0;
                    if (root.TryGetProperty("main", out var mainElement) && 
                        mainElement.TryGetProperty("temp", out var tempElement))
                    {
                        temperature = tempElement.GetDouble();
                    }

                    double feelsLike = 0;
                    if (root.TryGetProperty("main", out var mainElement2) && 
                        mainElement2.TryGetProperty("feels_like", out var feelsLikeElement))
                    {
                        feelsLike = feelsLikeElement.GetDouble();
                    }

                    string description = "Unknown";
                    if (root.TryGetProperty("weather", out var weatherElement) && 
                        weatherElement.GetArrayLength() > 0)
                    {
                        var firstWeather = weatherElement[0];
                        if (firstWeather.TryGetProperty("description", out var descElement))
                        {
                            description = descElement.GetString() ?? "Unknown";
                        }
                    }

                    int humidity = 0;
                    if (root.TryGetProperty("main", out var mainElement3) && 
                        mainElement3.TryGetProperty("humidity", out var humidityElement))
                    {
                        humidity = humidityElement.GetInt32();
                    }

                    double windSpeed = 0;
                    if (root.TryGetProperty("wind", out var windElement) && 
                        windElement.TryGetProperty("speed", out var speedElement))
                    {
                        windSpeed = speedElement.GetDouble();
                    }

                    int pressure = 0;
                    if (root.TryGetProperty("main", out var mainElement4) && 
                        mainElement4.TryGetProperty("pressure", out var pressureElement))
                    {
                        pressure = pressureElement.GetInt32();
                    }

                    int clouds = 0;
                    if (root.TryGetProperty("clouds", out var cloudsElement) && 
                        cloudsElement.TryGetProperty("all", out var allElement))
                    {
                        clouds = allElement.GetInt32();
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
                throw new InvalidOperationException("Invalid JSON response from weather API", ex);
            }
        }
    }
}
