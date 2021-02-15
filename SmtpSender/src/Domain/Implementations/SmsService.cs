using SmtpSender.Domain.Abstractions;
using SmtpSender.Domain.Models;
using SmtpSender.Domain.Services;
using System.Threading.Tasks;

namespace SmtpSender.Domain.Implementations
{
    internal class SmsService : ISmsService
    {
        private readonly ISendSms _smsSender;

        public SmsService(ISendSms smsSender)
        {
            _smsSender = smsSender;
        }

        /// <inheritdoc />
        public Task SendSmsAsync(SendSmsMessageIntent sms)
        {
            return _smsSender.SendSmsAsync(sms.Message);
        }
    }
}
