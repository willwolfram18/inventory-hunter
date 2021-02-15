using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmtpSender.Domain.Models;
using SmtpSender.Domain.Services;
using SmtpSender.WebApi.Models;

namespace SmtpSender.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{verion:apiVersion}/sms")]
    public class SmsController : ControllerBase
    {
        private readonly ISmsService _smsService;

        public SmsController(ISmsService smsService)
        {
            _smsService = smsService;
        }

        [HttpPost]
        public async Task<ActionResult> SendSms(SendSmsRequest request)
        {
            SendSmsMessageIntent intent = MapToDomainModel(request);

            await _smsService.SendSmsAsync(intent);

            return Ok();
        }

        private static SendSmsMessageIntent MapToDomainModel(SendSmsRequest request) =>
            new(new SmsMessage(request.ToPhoneNumber, request.Content));
    }
}
