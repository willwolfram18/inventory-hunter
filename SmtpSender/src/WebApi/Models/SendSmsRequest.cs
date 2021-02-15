namespace SmtpSender.WebApi.Models
{
    public class SendSmsRequest
    {
        public string ToPhoneNumber { get; set; }

        public string Content { get; set; }
    }
}
