using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly WeatherService _weatherService;

        public WeatherController(WeatherService weatherSerivce,ILogger<WeatherController> logger)
        {
            _logger = logger;
            _weatherService = weatherSerivce;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string country, [FromQuery] string city)
        {
            var weatherData = await _weatherService.GetWeatherDataAsync(country, city);

            return Ok(weatherData);
        }
    }
}
