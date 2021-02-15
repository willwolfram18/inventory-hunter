namespace SmtpSender.WebApi.Models
{
    public record EmailContent
    {
        public string Value { get; set; }

        public bool ContainsHtml { get; set; }
    }
}