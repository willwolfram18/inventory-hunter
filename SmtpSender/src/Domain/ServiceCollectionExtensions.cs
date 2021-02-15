using SmtpSender.Domain;
using SmtpSender.Domain.Implementations;
using Microsoft.Extensions.DependencyInjection;
using SmtpSender.Domain.Services;

namespace SmtpSender.Domain
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services)
        {
            return services.AddTransient<IEmailService, EmailService>();
        }

        public static IServiceCollection AddSmsService(this IServiceCollection services)
        {
            return services.AddTransient<ISmsService, SmsService>();
        }
    }
}