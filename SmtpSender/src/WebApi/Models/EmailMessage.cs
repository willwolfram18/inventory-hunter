using System;
using System.Collections.Generic;
using System.Linq;

namespace SmtpSender.WebApi.Models
{
    public class EmailMessage
    {
        public IEnumerable<EmailRecipient> Recipients { get; set; }

        public string Subject { get; set; }

        public EmailContent Content { get; set; }
    }
}