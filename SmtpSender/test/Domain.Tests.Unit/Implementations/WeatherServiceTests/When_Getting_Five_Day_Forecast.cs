using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using SmtpSender.Domain.Abstractions;
using SmtpSender.Domain.Implementations;
using SmtpSender.Domain.Models;
using NUnit.Framework;

namespace SmtpSender.Domain.Tests.Unit.Implementations.WeatherServiceTests
{
    public class When_Getting_Five_Day_Forecast
    {
#pragma warning disable CS8618
        private Mock<IWeatherForecastRepository> _mockWeatherForecastRepo;

        private WeatherService _systemUnderTest;
#pragma warning restore

        [SetUp]
        public void SetUp()
        {
            _mockWeatherForecastRepo = new Mock<IWeatherForecastRepository>();

            _systemUnderTest = new WeatherService(
                _mockWeatherForecastRepo.Object
            );
        }

        [Test]
        public async Task Then_Weather_Forecasts_Are_Retrieved_From_The_Repo()
        {
            const int numForecastsToRetrieve = 5;

            // Given a set of fake weather forecasts...
            List<WeatherForecast> fakeForecasts = CreateFakeWeatherForecasts(numForecastsToRetrieve);

            // Given the weather forecast repo returns the fake weather forecasts...
            _mockWeatherForecastRepo.Setup(repo => repo.GetWeatherForecastAsync(numForecastsToRetrieve))
                .ReturnsAsync(fakeForecasts)
                .Verifiable();

            // When getting the five day forecast...
            IEnumerable<WeatherForecast> result = await _systemUnderTest.GetFiveDayForecastAsync();

            // Then the forecasts from the repo are returned...
            result.Should().NotBeNull().And.BeEquivalentTo(fakeForecasts);

            // Then the repo was invoked.
            _mockWeatherForecastRepo.Verify();
            _mockWeatherForecastRepo.VerifyNoOtherCalls();
        }

        private static List<WeatherForecast> CreateFakeWeatherForecasts(int numForecasts)
        {
            return Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast(
                    DateTime.Now.AddDays(index),
                    index
                )
                {
                    Summary = $"Summary {index}"
                }
            ).ToList();
        }
    }
}