namespace SmtpSender.Domain.Models
{
    public class EmailRecipient
    {
        public EmailRecipient(string emailAddress, string? name)
        {
            EmailAddress = emailAddress;
            Name = name;
        }

        public string EmailAddress { get; }

        public string? Name { get; }
    }
}