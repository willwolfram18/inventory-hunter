using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using SmtpSender.Domain.Abstractions;
using SmtpSender.Domain.Models;
using SmtpSender.Infrastructure.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmtpSender.Infrastructure.Implementations
{
    internal class SendGridEmailSender : ISendEmails
    {
        private readonly ISendGridClient _client;
        private readonly IOptions<EmailSettings> _settings;

        public SendGridEmailSender(ISendGridClient client,
            IOptions<EmailSettings> settings)
        {
            _client = client;
            _settings = settings;
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            SendGridMessage messageToSend = MapToSendGridMessage(message);

            Response response = await _client.SendEmailAsync(messageToSend);

            if (((int)response.StatusCode) is >= 200 and < 300)
            {
                return;
            }

            throw new Exception(await response.Body.ReadAsStringAsync());
        }

        private SendGridMessage MapToSendGridMessage(EmailMessage message)
        {
            var sendGridMessage = new SendGridMessage
            {
                Subject = message.Subject,
                From = new EmailAddress(_settings.Value.FromAddress, _settings.Value.FromName),
            };

            sendGridMessage.AddTos(message.Recipients.Select(MapToSendGridEmailAddress).ToList());

            if (message.Content.ContainsHtml)
            {
                sendGridMessage.HtmlContent = message.Content.Value;
            }
            else
            {
                sendGridMessage.PlainTextContent = message.Content.Value;
            }

            return sendGridMessage;
        }

        private static EmailAddress MapToSendGridEmailAddress(EmailRecipient recipient) =>
            new(recipient.EmailAddress, recipient.Name);
    }
}