using SmtpSender.Domain;
using SmtpSender.Domain.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace SmtpSender.Domain
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeatherServices(this IServiceCollection services)
        {
            return services.AddTransient<IWeatherService, WeatherService>();
        }
    }
}