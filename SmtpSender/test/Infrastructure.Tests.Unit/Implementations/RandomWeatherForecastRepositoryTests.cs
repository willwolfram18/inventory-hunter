using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using SmtpSender.Domain.Models;
using SmtpSender.Infrastructure.Implementations;
using NUnit.Framework;

namespace SmtpSender.Infrastructure.Tests.Unit.Implementations
{
    public class RandomWeatherForecastRepositoryTests
    {
#pragma warning disable CS8618
        private RandomWeatherForecastRepository _systemUnderTest;
#pragma warning restore

        [SetUp]
        public void SetUp()
        {
            _systemUnderTest = new RandomWeatherForecastRepository();
        }

        [Test]
        public async Task When_Getting_Weather_Forecasts_Then_Only_The_Number_Of_Requested_Forecasts_Is_Returned(
            [Values(0, 1, 2, 10)] int numForecastsToRetrieve
        )
        {
            // When retrieving weather forecasts...
            IEnumerable<WeatherForecast> forecasts = await _systemUnderTest.GetWeatherForecastAsync(numForecastsToRetrieve);

            // Then only requested number of forecasts are returned.
            forecasts.Should().NotBeNull().And.HaveCount(numForecastsToRetrieve);
        }

        [Test]
        public async Task When_Getting_Weather_Forecasts_Then_All_Days_Are_In_The_Future()
        {
            const int numForecastsToRetrieve = 5;

            // When retrieving weather forecasts...
            IEnumerable<WeatherForecast> forecasts = await _systemUnderTest.GetWeatherForecastAsync(numForecastsToRetrieve);

            // Then all forecasts are in the future.
            forecasts.Should().Match(actualForecasts => actualForecasts.All(forecast => forecast.Date > DateTime.Today));
        }
    }
}