using API;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Test.WeatherServiceTest
{
    [TestFixture]
    public class WeatherServiceTests
    {
        private IOptions<AppSettings> _mockOptions;
        private WeatherService _weatherService;

        [SetUp]
        public void SetUp()
        {
            // Arrange
            var appSettings = new AppSettings { OpenWeatherAPIKeys = "Enter API Key" };
            _mockOptions = Substitute.For<IOptions<AppSettings>>();
            _mockOptions.Value.Returns(appSettings);

            _weatherService = new WeatherService(_mockOptions);
        }

        [Test]
        public async Task GetWeatherDataAsync_ValidResponse_ReturnsWeatherResponse()
        {
            // Act
            var result = await _weatherService.GetWeatherDataAsync("Australia", "Melbourne");

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Weather);
        }
    }
}
