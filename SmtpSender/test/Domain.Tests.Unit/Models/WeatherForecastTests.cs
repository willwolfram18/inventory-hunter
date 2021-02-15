using System;
using FluentAssertions;
using SmtpSender.Domain.Models;
using NUnit.Framework;

namespace SmtpSender.Domain.Tests.Unit.Models
{
    public class WeatherForecastTests
    {
        [Test]
        public void When_Creating_A_Weather_Forecast_Then_Date_And_Temperature_In_Celcius_Should_Match_Parameters()
        {
            // Given a forecast date and temperature...
            DateTime forecastDate = DateTime.Now.AddDays(10);
            const int temperature = 37;

            // When creating a weather forecast...
            var model = new WeatherForecast(forecastDate, temperature);


            // Then the date and temperature in Celcius should match the parameters.
            model.Date.Should().Be(forecastDate);
            model.TemperatureC.Should().Be(temperature);
            model.Summary.Should().BeNull();
        }

        [Test]
        public void When_Creating_A_Weather_Forecasts_Then_Temperature_In_Fahrenheit_Should_Match_The_Converted_Celsius_Temperature()
        {
            // Given a temperature in Celcius...
            const int temperatureInCelcius = 28;

            // When creating a weather forecast with that temperature...
            var model = new WeatherForecast(DateTime.Today, temperatureInCelcius);

            // Then the temperature in Fahrenheit is the converted temperature in Celcius.
            model.TemperatureF.Should().Be(ConvertToFahrenheit(temperatureInCelcius));
        }

        [Test]
        public void When_Creating_A_Weather_Forecast_Then_A_Summary_Can_Be_Provided()
        {
            // Given a weather forecast summary...
            const string expectedSummary = "This is the weather summary.";

            // When creating a weather forecast...
            var model = new WeatherForecast(DateTime.Today, 33)
            {
                Summary = expectedSummary
            };

            // Then the summary matches.
            model.Summary.Should().Be(expectedSummary);
        }

        private static int ConvertToFahrenheit(int tempInCelcius) => 32 + (int)(tempInCelcius / 0.5556);
    }
}