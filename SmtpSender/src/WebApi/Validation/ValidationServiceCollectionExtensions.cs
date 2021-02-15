using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using SmtpSender.WebApi.Models;

namespace SmtpSender.WebApi.Validation
{
    public static class ValidationServiceCollectionExtensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services.AddTransient<IValidator<EmailMessageRequest>, EmailMessageRequestValidator>();
        }
    }
}
