using System;

namespace SmtpSender.Domain.Models
{
    public class SmsMessage
    {
        public SmsMessage(string toPhoneNumber, string content)
        {
            if (string.IsNullOrWhiteSpace(toPhoneNumber))
            {
                throw new ArgumentException("Must provide a phone number.", nameof(toPhoneNumber));
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Must provide message content", nameof(content));
            }

            ToPhoneNumber = toPhoneNumber;
            Content = content;
        }

        public string ToPhoneNumber { get; }

        public string Content { get; }
    }
}
