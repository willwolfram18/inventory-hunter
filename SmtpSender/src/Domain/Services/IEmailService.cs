using System.Threading.Tasks;
using SmtpSender.Domain.Models;

namespace SmtpSender.Domain.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage message);
    }
}