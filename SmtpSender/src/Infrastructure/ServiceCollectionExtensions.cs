using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;
using SmtpSender.Domain.Abstractions;
using SmtpSender.Infrastructure.SendGrid.Implementations;
using SmtpSender.Infrastructure.SendGrid.Settings;
using SmtpSender.Infrastructure.Twilio.Settings;
using Twilio.Clients;
using Twilio.Http;
using HttpClient = System.Net.Http.HttpClient;

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

        public static IServiceCollection AddTwilioSmsSender(this IServiceCollection services, SmsSettings settings)
        {
            const string TwilioHttpClient = "TwilioClient";

            services.AddHttpClient(TwilioHttpClient);

            services.AddTransient<ITwilioRestClient, TwilioRestClient>(serviceProvider =>
            {
                HttpClient client = serviceProvider.GetRequiredService<IHttpClientFactory>()
                    .CreateClient(TwilioHttpClient);

                return new TwilioRestClient(settings.TwilioAccountSid, settings.TwilioAuthToken,
                    httpClient: new SystemNetHttpClient(client));
            });

            services.AddOptions<SmsSettings>()
                .Configure(optionsInstance =>
                {
                    optionsInstance.FromPhoneNumber = settings.FromPhoneNumber;
                    optionsInstance.TwilioAccountSid = settings.TwilioAccountSid;
                    optionsInstance.TwilioAuthToken = settings.TwilioAuthToken;
                });

            return services;
        }
    }
}