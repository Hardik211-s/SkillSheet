using Microsoft.AspNetCore.Mvc;
using SkillSheetAPI.Services.Interfaces;
using SkillSheetAPI.Resources;

namespace SkillSheetAPI.Controllers
{
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
            _weatherService = weatherService;
        }

        /// <summary>
        /// Gets weather information for a specific city.
        /// </summary>
        /// <param name="city">The name of the city.</param>
        /// <returns>An <see cref="IActionResult"/> containing the weather data.</returns>
        [HttpGet("city")]
        public async Task<IActionResult> GetWeatherByCity([FromQuery] string city)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(city))
                {
                    return BadRequest(new { message = "City name is required." });
                }

                var weather = await _weatherService.GetWeatherByCityAsync(city);
                return Ok(new { message = "Weather data retrieved successfully.", data = weather });
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
        public async Task<IActionResult> GetWeatherByCoordinates([FromQuery] double latitude, [FromQuery] double longitude)
        {
            try
            {
                if (latitude < -90 || latitude > 90 || longitude < -180 || longitude > 180)
                {
                    return BadRequest(new { message = "Invalid latitude or longitude values." });
                }

                var weather = await _weatherService.GetWeatherByCoordinatesAsync(latitude, longitude);
                return Ok(new { message = "Weather data retrieved successfully.", data = weather });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
