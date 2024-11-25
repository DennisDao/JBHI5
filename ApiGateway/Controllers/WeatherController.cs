using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System;
using System.Text.Json;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ILogger<WeatherController> _logger;
        private readonly AppSettings _appSettings;

        public WeatherController(IOptions<AppSettings> appSettings, ILogger<WeatherController> logger)
        {
            _logger = logger;
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string country, [FromQuery] string city)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync($"{_appSettings.ApiUrl}/Weather?country={country}&city={city}");
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                WeatherResponse weatherResponse = JsonSerializer.Deserialize<WeatherResponse>(jsonResponse);

                if (weatherResponse?.Weather != null && weatherResponse.Weather.Count > 0)
                {
                    return Ok(weatherResponse.Weather[0].Description);
                }
            }

            return BadRequest("Unable to retrieve weather data");
        }
    }
}
