using Microsoft.AspNetCore.Mvc;
using SmtpSender.Domain.Models;
using SmtpSender.Domain.Services;
using System.Linq;
using System.Threading.Tasks;

namespace SmtpSender.WebApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/emails")]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailsController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<ActionResult> SendEmail(Models.EmailMessageRequest message)
        {
            EmailMessage emailMessageIntent = MapToDomainModel(message);

            await _emailService.SendEmailAsync(emailMessageIntent);

            return Ok();
        }

        private static EmailMessage MapToDomainModel(Models.EmailMessageRequest message) =>
            new EmailMessage(
                message.Recipients.Select(MapToDomainModel),
                message.Subject,
                MapToDomainModel(message.Content)
            );

        private static EmailRecipient MapToDomainModel(Models.EmailRecipient recipient) =>
            new(recipient.EmailAddress, recipient.Name);

        private static EmailContent MapToDomainModel(Models.EmailContent content) =>
            new(content.Value, content.ContainsHtml);
    }
}