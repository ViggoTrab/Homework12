using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;

namespace Homework12.Tests
{
    public class WeatherServiceTests
    {
        public interface IWeatherApiClient
        {
            Task<WeatherResponse> GetWeatherDataAsync(string city);
        }

        public class WeatherService
        {
            private readonly IWeatherApiClient _weatherApiClient;

            public WeatherService(IWeatherApiClient weatherApiClient)
            {
                _weatherApiClient = weatherApiClient;
            }

            public async Task<string> GetWeatherDataAsync(string city)
            {
                try
                {
                    var weatherResponse = await _weatherApiClient.GetWeatherDataAsync(city);
                    if (weatherResponse?.current != null)
                    {
                        return $"Weather for {weatherResponse.location.name}: " +
                               $"Temperature {weatherResponse.current.tempc}°C, " +
                               $"Wind Speed {weatherResponse.current.windkph} km/h.";
                    }
                    return "Weather data not available.";
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException("Error fetching weather data", ex);
                }
            }
        }

        public class WeatherServiceTests
        {
            [Fact]
            public async Task GetWeatherDataAsync_ShouldReturnCorrectWeatherData()
            {
                // Arrange
                var mockApiClient = new Mock<IWeatherApiClient>();
                var mockWeatherResponse = new WeatherResponse
                {
                    location = new Location { name = "New York", country = "USA" },
                    current = new CurrentWeather { tempc = 22.5f, windkph = 15.5f }
                };

                // Mock the method to return the weather data
                mockApiClient.Setup(x => x.GetWeatherDataAsync("New York"))
                    .ReturnsAsync(mockWeatherResponse);

                var weatherService = new WeatherService(mockApiClient.Object);

                // Act
                var result = await weatherService.GetWeatherDataAsync("New York");

                // Assert
                Assert.Equal("Weather for New York: Temperature 22.5°C, Wind Speed 15.5 km/h.", result);
            }

            [Fact]
            public async Task GetWeatherDataAsync_ShouldReturnNoDataMessage_WhenWeatherDataIsNull()
            {
                // Arrange
                var mockApiClient = new Mock<IWeatherApiClient>();
                var mockWeatherResponse = new WeatherResponse
                {
                    location = new Location { name = "New York", country = "USA" },
                    current = null  // Simulate no current weather data
                };

                // Mock the method to return the incomplete weather data
                mockApiClient.Setup(x => x.GetWeatherDataAsync("New York"))
                    .ReturnsAsync(mockWeatherResponse);

                var weatherService = new WeatherService(mockApiClient.Object);

                // Act
                var result = await weatherService.GetWeatherDataAsync("New York");

                // Assert
                Assert.Equal("Weather data not available.", result);
            }
        }

        [Fact]  // Marking the test method as a fact (test)
        public async Task GetWeatherDataAsync_ShouldThrowException_WhenApiFails()
        {
            // Arrange
            var mockApiClient = new Mock<IWeatherApiClient>();

            // Mock the method to throw an exception
            mockApiClient.Setup(x => x.GetWeatherDataAsync("New York"))
                .ThrowsAsync(new InvalidOperationException("API call failed"));

            var weatherService = new WeatherService(mockApiClient.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await weatherService.GetWeatherDataAsync("New York")
            );

            Assert.Equal("Error fetching weather data", ex.Message);
        }
    }
}     

