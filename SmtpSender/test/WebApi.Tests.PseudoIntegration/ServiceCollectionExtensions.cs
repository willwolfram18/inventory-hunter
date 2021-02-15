using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace SmtpSender.WebApi.Tests.PseudoIntegration
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection ReplaceServiceWithMock<TService>(this IServiceCollection services) where TService : class
        {
            return services.ReplaceServiceWithMock(new Mock<TService>());
        }

        public static IServiceCollection ReplaceServiceWithMock<TService>(this IServiceCollection services, Mock<TService> mock) where TService : class
        {
            services.RemoveAll<TService>();
            return services.AddSingleton<TService>(mock.Object);
        }
    }
}