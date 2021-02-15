using SmtpSender.Domain.Abstractions;
using SmtpSender.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmtpSender.Domain.Implementations
{
    internal class WeatherService : IWeatherService
    {
        private readonly IWeatherForecastRepository _weatherRepo;

        public WeatherService(IWeatherForecastRepository weatherRepo)
        {
            _weatherRepo = weatherRepo;
        }

        public Task<IEnumerable<WeatherForecast>> GetFiveDayForecastAsync()
        {
            return _weatherRepo.GetWeatherForecastAsync(5);
        }
    }
}