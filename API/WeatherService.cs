using Microsoft.Extensions.Options;
using System.Text.Json;

namespace API
{
    public class WeatherService
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private readonly AppSettings _appSettings;

        public WeatherService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public async Task<string> GetWeatherDataAsync(string country, string city)
        {
            string apiKey = _appSettings.OpenWeatherAPIKeys;
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={country},{city}&appid={apiKey}";
            HttpResponseMessage response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                WeatherResponse weatherResponse = JsonSerializer.Deserialize<WeatherResponse>(jsonResponse);

                if (weatherResponse?.Weather != null && weatherResponse.Weather.Count > 0)
                {
                    return weatherResponse.Weather[0].Description;
                }
            }

            return "";
        }
    }
}
