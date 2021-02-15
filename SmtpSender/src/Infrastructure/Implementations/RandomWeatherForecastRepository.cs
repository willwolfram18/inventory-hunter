using SmtpSender.Domain.Abstractions;
using SmtpSender.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmtpSender.Infrastructure.Implementations
{
    internal class RandomWeatherForecastRepository : IWeatherForecastRepository
    {
        private static readonly Random Rng = new Random();

        private static readonly string?[] Summaries = new[]
        {
            null, "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public Task<IEnumerable<WeatherForecast>> GetWeatherForecastAsync(int numDays)
        {
            IEnumerable<WeatherForecast> forecasts = Enumerable.Range(1, numDays).Select(index =>
                new WeatherForecast(
                    DateTime.Now.AddDays(index),
                    Rng.Next(-20, 55)
                )
                {
                    Summary = Summaries[Rng.Next(Summaries.Length)]
                }
            )
            .ToArray();

            return new ValueTask<IEnumerable<WeatherForecast>>(forecasts).AsTask();
        }
    }
}