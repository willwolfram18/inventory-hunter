using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmtpSender.Domain.Models;

namespace SmtpSender.Domain.Services
{
    public interface ISmsService
    {
        Task SendSmsAsync(SendSmsMessageIntent sms);
    }
}
