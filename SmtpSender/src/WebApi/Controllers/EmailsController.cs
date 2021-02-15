using System;
using Microsoft.AspNetCore.Mvc;
using SmtpSender.Domain.Services;

namespace SmtpSender.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/{version:apiVersion}/emails")]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailsController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public ActionResult SendEmail(Models.EmailMessage message)
        {
            throw new NotImplementedException();
        }
    }
}