using FluentValidation;
using SmtpSender.WebApi.Models;

namespace SmtpSender.WebApi.Validation
{
    public class EmailRecipientValidator : AbstractValidator<EmailRecipient>
    {
        public EmailRecipientValidator()
        {
            RuleFor(subject => subject.EmailAddress)
                .EmailAddress();
        }
    }
}
