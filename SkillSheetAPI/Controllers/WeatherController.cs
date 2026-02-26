using Microsoft.AspNetCore.Mvc;
using SkillSheetAPI.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SkillSheetAPI.Controllers
{
    /// <summary>
    /// Controller for weather-related API endpoints.
    /// </summary>
    [ApiController]
    [Route("api/weather")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherController"/> class.
        /// </summary>
        /// <param name="weatherService">The weather service.</param>
        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService ?? throw new ArgumentNullException(nameof(weatherService));
        }

        /// <summary>
        /// Gets weather information for a specific city.
        /// </summary>
        /// <param name="city">The name of the city.</param>
        /// <returns>An <see cref="IActionResult"/> containing the weather data.</returns>
        [HttpGet("city")]
        [Produces("application/json")]
        public async Task<IActionResult> GetWeatherByCity([FromQuery] string city)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(city))
                {
                    return BadRequest(new { message = "City name is required and cannot be empty." });
                }

                var weather = await _weatherService.GetWeatherByCityAsync(city);
                return Ok(new { message = "Weather data retrieved successfully.", data = weather });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Gets weather information for specific coordinates.
        /// </summary>
        /// <param name="latitude">The latitude coordinate.</param>
        /// <param name="longitude">The longitude coordinate.</param>
        /// <returns>An <see cref="IActionResult"/> containing the weather data.</returns>
        [HttpGet("coordinates")]
        [Produces("application/json")]
        public async Task<IActionResult> GetWeatherByCoordinates([FromQuery] double latitude, [FromQuery] double longitude)
        {
            try
            {
                if (latitude < -90 || latitude > 90)
                {
                    return BadRequest(new { message = "Latitude must be between -90 and 90." });
                }

                if (longitude < -180 || longitude > 180)
                {
                    return BadRequest(new { message = "Longitude must be between -180 and 180." });
                }

                var weather = await _weatherService.GetWeatherByCoordinatesAsync(latitude, longitude);
                return Ok(new { message = "Weather data retrieved successfully.", data = weather });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
