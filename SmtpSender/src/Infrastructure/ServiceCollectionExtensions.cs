using SmtpSender.Domain.Abstractions;
using SmtpSender.Infrastructure.Implementations;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;

namespace SmtpSender.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSendGridEmailSender(this IServiceCollection services, string apiKey)
        {
            services.AddSendGrid(options =>
            {
                options.ApiKey = apiKey;
            });

            return services.AddTransient<ISendEmails, SendGridEmailSender>();
        }
    }
}