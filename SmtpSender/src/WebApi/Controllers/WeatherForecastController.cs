using SmtpSender.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SmtpSender.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherService _weatherService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,
            IWeatherService weatherService)
        {
            _logger = logger;
            _weatherService = weatherService;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            return (await _weatherService.GetFiveDayForecastAsync()).Select(MapToApiContract);
        }

        private static WeatherForecast MapToApiContract(Domain.Models.WeatherForecast model)
        {
            return new WeatherForecast
            {
                Date = model.Date,
                Summary = model.Summary,
                TemperatureC = model.TemperatureC,
                TemperatureF = model.TemperatureF
            };
        }
    }
}
