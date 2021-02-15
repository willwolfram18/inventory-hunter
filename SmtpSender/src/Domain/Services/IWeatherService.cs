using SmtpSender.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmtpSender.Domain
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherForecast>> GetFiveDayForecastAsync();
    }
}