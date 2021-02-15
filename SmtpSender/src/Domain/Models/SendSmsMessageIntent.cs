using System;

namespace SmtpSender.Domain.Models
{
    public class SendSmsMessageIntent
    {
        public SendSmsMessageIntent(SmsMessage message)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }

        public SmsMessage Message { get; }
    }
}
