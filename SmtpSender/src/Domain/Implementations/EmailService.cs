using System.Threading.Tasks;
using SmtpSender.Domain.Abstractions;
using SmtpSender.Domain.Models;
using SmtpSender.Domain.Services;

namespace SmtpSender.Domain.Implementations
{
    internal class EmailService : IEmailService
    {
        private readonly ISendEmails _emailSender;

        public EmailService(ISendEmails emailSender)
        {
            _emailSender = emailSender;
        }

        public Task SendEmailAsync(EmailMessage message)
        {
            return _emailSender.SendEmailAsync(message);
        }
    }
}