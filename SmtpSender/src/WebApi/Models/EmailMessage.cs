using System;
using System.Collections.Generic;
using System.Linq;

namespace SmtpSender.WebApi.Models
{
    public record EmailMessageRequest
    {
#pragma warning disable CS8618
        public IEnumerable<EmailRecipient> Recipients { get; set; }

        public string? Subject { get; set; }

        public EmailContent Content { get; set; }
#pragma warning restore
    }
}