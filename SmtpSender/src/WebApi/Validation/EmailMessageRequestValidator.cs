using FluentValidation;
using SmtpSender.WebApi.Models;

namespace SmtpSender.WebApi.Validation
{
    public class EmailMessageRequestValidator : AbstractValidator<EmailMessageRequest>
    {
        public EmailMessageRequestValidator(IValidator<EmailRecipient> emailRecipientValidator)
        {
            RuleFor(request => request.Recipients)
                .NotEmpty()
                .WithMessage("The {PropertyName} field is required.")
                .When(subject => subject.Recipients != null);

            RuleForEach(request => request.Recipients)
                .Must(recipient => recipient != null)
                .WithMessage("The {PropertyName} field cannot contain nulls.")
                .When(subject => subject.Recipients != null);

            RuleForEach(request => request.Recipients)
                .SetValidator(emailRecipientValidator);
        }
    }
}
