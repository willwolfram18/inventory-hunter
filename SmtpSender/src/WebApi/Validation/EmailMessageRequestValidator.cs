using FluentValidation;
using SmtpSender.WebApi.Models;

namespace SmtpSender.WebApi.Validation
{
    public class EmailMessageRequestValidator : AbstractValidator<EmailMessageRequest>
    {
        public EmailMessageRequestValidator()
        {
            RuleFor(request => request.Recipients)
                .NotEmpty()
                .WithMessage("The {PropertyName} field is required.")
                .When(subject => subject.Recipients != null);
        }
    }
}
