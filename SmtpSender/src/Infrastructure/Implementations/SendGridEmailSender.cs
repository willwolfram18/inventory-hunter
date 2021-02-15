using System.Threading.Tasks;
using SendGrid;
using SmtpSender.Domain.Abstractions;
using SmtpSender.Domain.Models;

namespace SmtpSender.Infrastructure.Implementations
{
    internal class SendGridEmailSender : ISendEmails
    {
        private readonly ISendGridClient _client;

        public SendGridEmailSender(ISendGridClient client)
        {
            _client = client;
        }

        public Task SendEmailAsync(EmailMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}