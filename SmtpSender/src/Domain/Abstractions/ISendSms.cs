using SmtpSender.Domain.Models;
using System.Threading.Tasks;

namespace SmtpSender.Domain.Abstractions
{
    public interface ISendSms
    {
        Task SendSmsAsync(SmsMessage message);
    }
}
