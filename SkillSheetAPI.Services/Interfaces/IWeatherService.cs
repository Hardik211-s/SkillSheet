namespace SkillSheetAPI.Services.Interfaces
{
    /// <summary>
    /// Interface for weather service operations.
    /// </summary>
    public interface IWeatherService
    {
        /// <summary>
        /// Gets weather information for a specific city.
        /// </summary>
        /// <param name="city">The name of the city.</param>
        /// <returns>A task that represents the asynchronous operation and contains weather data.</returns>
        Task<WeatherData> GetWeatherByCityAsync(string city);

        /// <summary>
        /// Gets weather information for coordinates (latitude and longitude).
        /// </summary>
        /// <param name="latitude">The latitude coordinate.</param>
        /// <param name="longitude">The longitude coordinate.</param>
        /// <returns>A task that represents the asynchronous operation and contains weather data.</returns>
        Task<WeatherData> GetWeatherByCoordinatesAsync(double latitude, double longitude);
    }

    /// <summary>
    /// Represents weather data.
    /// </summary>
    public class WeatherData
    {
        /// <summary>
        /// Gets or sets the city name.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the country code.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the current temperature in Celsius.
        /// </summary>
        public double Temperature { get; set; }

        /// <summary>
        /// Gets or sets the feels like temperature in Celsius.
        /// </summary>
        public double FeelsLike { get; set; }

        /// <summary>
        /// Gets or sets the weather description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the humidity percentage.
        /// </summary>
        public int Humidity { get; set; }

        /// <summary>
        /// Gets or sets the wind speed in m/s.
        /// </summary>
        public double WindSpeed { get; set; }

        /// <summary>
        /// Gets or sets the pressure in hPa.
        /// </summary>
        public int Pressure { get; set; }

        /// <summary>
        /// Gets or sets the cloudiness percentage.
        /// </summary>
        public int Clouds { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of the weather data.
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
