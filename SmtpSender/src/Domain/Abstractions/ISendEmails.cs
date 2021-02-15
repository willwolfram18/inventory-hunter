using System.Threading.Tasks;
using SmtpSender.Domain.Models;

namespace SmtpSender.Domain.Abstractions
{
    public interface ISendEmails
    {
        Task SendEmailAsync(EmailMessage message);
    }
}