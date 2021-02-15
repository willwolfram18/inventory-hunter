using SmtpSender.Domain.Abstractions;
using SmtpSender.Infrastructure.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace SmtpSender.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWeatherForecastRepository(this IServiceCollection services)
        {
            return services.AddTransient<IWeatherForecastRepository, RandomWeatherForecastRepository>();
        }
    }
}