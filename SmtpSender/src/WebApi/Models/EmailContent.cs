namespace SmtpSender.WebApi.Models
{
    public class EmailContent
    {
        public string Value { get; set; }

        public bool ContainsHtml { get; set; }
    }
}