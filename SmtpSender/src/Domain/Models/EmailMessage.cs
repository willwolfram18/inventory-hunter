using System;
using System.Collections.Generic;
using System.Linq;

namespace SmtpSender.Domain.Models
{
    public class EmailMessage
    {
        public EmailMessage(IEnumerable<EmailRecipient> recipients, string? subject, EmailContent content)
        {
            Recipients = recipients ?? throw new ArgumentNullException(nameof(recipients));
            Subject = subject;
            Content = content ?? throw new ArgumentNullException(nameof(content));

            if (!Recipients.Any())
            {
                throw new ArgumentException("Must have at least one recipient.", nameof(recipients));
            }
        }

        public IEnumerable<EmailRecipient> Recipients { get; }

        public string? Subject { get; }

        public EmailContent Content { get; }
    }
}