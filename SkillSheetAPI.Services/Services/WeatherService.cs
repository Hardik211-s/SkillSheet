using SkillSheetAPI.Services.Interfaces;
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
            _httpClient = httpClient;
            _apiKey = apiKey;
        }

        /// <summary>
        /// Gets weather information for a specific city.
        /// </summary>
        /// <param name="city">The name of the city.</param>
        /// <returns>A task that represents the asynchronous operation and contains weather data.</returns>
        public async Task<WeatherData> GetWeatherByCityAsync(string city)
        {
            try
            {
                var url = $"{BaseUrl}?q={city}&units=metric&appid={_apiKey}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to retrieve weather data. Status code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                return ParseWeatherResponse(content);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching weather for city {city}: {ex.Message}");
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
            try
            {
                var url = $"{BaseUrl}?lat={latitude}&lon={longitude}&units=metric&appid={_apiKey}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to retrieve weather data. Status code: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                return ParseWeatherResponse(content);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching weather for coordinates ({latitude}, {longitude}): {ex.Message}");
            }
        }

        /// <summary>
        /// Parses the weather API response.
        /// </summary>
        /// <param name="jsonContent">The JSON response content from the API.</param>
        /// <returns>A <see cref="WeatherData"/> object containing parsed weather information.</returns>
        private WeatherData ParseWeatherResponse(string jsonContent)
        {
            using (JsonDocument doc = JsonDocument.Parse(jsonContent))
            {
                var root = doc.RootElement;

                var weatherData = new WeatherData
                {
                    City = root.GetProperty("name").GetString(),
                    Country = root.GetProperty("sys").GetProperty("country").GetString(),
                    Temperature = root.GetProperty("main").GetProperty("temp").GetDouble(),
                    FeelsLike = root.GetProperty("main").GetProperty("feels_like").GetDouble(),
                    Description = root.GetProperty("weather")[0].GetProperty("description").GetString(),
                    Humidity = root.GetProperty("main").GetProperty("humidity").GetInt32(),
                    WindSpeed = root.GetProperty("wind").GetProperty("speed").GetDouble(),
                    Pressure = root.GetProperty("main").GetProperty("pressure").GetInt32(),
                    Clouds = root.GetProperty("clouds").GetProperty("all").GetInt32(),
                    Timestamp = DateTime.UtcNow
                };

                return weatherData;
            }
        }
    }
}
