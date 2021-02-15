namespace SmtpSender.WebApi.Models
{
    public record EmailRecipient
    {
        public string EmailAddress { get; set; }

        public string Name { get; set; }
    }
}