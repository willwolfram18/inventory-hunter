using SmtpSender.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmtpSender.Domain.Abstractions
{
    public interface IWeatherForecastRepository
    {
        Task<IEnumerable<WeatherForecast>> GetWeatherForecastAsync(int numDays);
    }
}