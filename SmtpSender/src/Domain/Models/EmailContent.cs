namespace SmtpSender.Domain.Models
{
    public class EmailContent
    {
        public EmailContent(string value, bool containsHtml)
        {
            Value = value;
            ContainsHtml = containsHtml;
        }

        public string Value { get; }

        public bool ContainsHtml { get; }
    }
}