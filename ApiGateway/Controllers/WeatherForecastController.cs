using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static LinkedList requestHistory = new LinkedList();
        private static RateLimitRule rule = new RateLimitRule()
        {
            ApiKey = "ABC-123",
            EndPoint = "",
            Limit = 2,
            Window = TimeSpan.FromSeconds(5)
        };


        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var head = requestHistory.GetHead();

            if(head == null) 
            {
                requestHistory.AddLast(DateTime.Now);
            }
            else
            {
                var totalRequest = requestHistory.Count();
                DateTime currentTime = DateTime.Now;

                double totalMinutesElapsed = (currentTime - head.TimeStamp).TotalMinutes;
                var isWithinWindow = totalMinutesElapsed <= rule.Window.TotalMinutes;

                if(totalRequest == rule.Limit && isWithinWindow)
                {
                    return StatusCode(429, "Too many request");
                }

                if(totalRequest == rule.Limit && !isWithinWindow)
                {
                    requestHistory.RemoveHead();
                    requestHistory.AddLast(DateTime.Now);
                }
                else
                {
                    requestHistory.AddLast(DateTime.Now);
                }
            }

            return Ok(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray());
        }


    }


}
