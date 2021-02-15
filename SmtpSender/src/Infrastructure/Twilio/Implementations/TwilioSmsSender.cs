using Microsoft.Extensions.Options;
using SmtpSender.Domain.Abstractions;
using SmtpSender.Domain.Models;
using SmtpSender.Infrastructure.Twilio.Settings;
using System;
using System.Threading.Tasks;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SmtpSender.Infrastructure.Twilio.Implementations
{
    internal class TwilioSmsSender : ISendSms
    {
        private readonly ITwilioRestClient _client;
        private readonly IOptions<SmsSettings> _settings;

        public TwilioSmsSender(ITwilioRestClient client,
            IOptions<SmsSettings> settings)
        {
            _client = client;
            _settings = settings;
        }

        /// <inheritdoc />
        public async Task SendSmsAsync(SmsMessage message)
        {
            var twilioMessage = await MessageResource.CreateAsync(
                to: new PhoneNumber(message.ToPhoneNumber),
                from: new PhoneNumber(_settings.Value.FromPhoneNumber),
                body: message.Content,
                client: _client
            );

            if (twilioMessage.ErrorCode != null)
            {
                throw new Exception(twilioMessage.ErrorMessage);
            }
        }
    }
}
