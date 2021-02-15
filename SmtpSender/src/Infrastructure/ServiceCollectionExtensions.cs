using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;
using SmtpSender.Domain.Abstractions;
using SmtpSender.Infrastructure.SendGrid.Implementations;
using SmtpSender.Infrastructure.SendGrid.Settings;

namespace SmtpSender.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSendGridEmailSender(this IServiceCollection services, EmailSettings settings)
        {
            services.AddSendGrid(sendGridOptions =>
            {
                sendGridOptions.ApiKey = settings.SendGridApiKey;
            });

            services.AddOptions<EmailSettings>()
                .Configure(optionsInstance =>
                {
                    optionsInstance.SendGridApiKey = settings.SendGridApiKey;
                    optionsInstance.FromAddress = settings.FromAddress;
                    optionsInstance.FromName = settings.FromName;
                });

            return services.AddTransient<ISendEmails, SendGridEmailSender>();
        }
    }
}