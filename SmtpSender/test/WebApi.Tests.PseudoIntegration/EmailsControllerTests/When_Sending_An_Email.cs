using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SendGrid;
using SmtpSender.WebApi.Models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmtpSender.WebApi.Tests.PseudoIntegration.EmailsControllerTests
{
    class When_Sending_An_Email
    {
#pragma warning disable CS8618
        private CustomWebApplicationFactory _factory;
        private Mock<ISendGridClient> _sendGridMock;
#pragma warning restore

        [SetUp]
        public void SetUp()
        {
            _sendGridMock = new Mock<ISendGridClient>();

            _factory = new CustomWebApplicationFactory(services =>
            {
                services.ReplaceServiceWithMock(_sendGridMock);
            });
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }

        public static IEnumerable<IEnumerable<EmailRecipient>?> NullOrEmptyRecipients =>
         new IEnumerable<EmailRecipient>?[]
         {
             null,
             new EmailRecipient[0]
         };

        [Test]
        public async Task Then_Recipients_List_Cannot_Be_Null_Or_Empty(
            [ValueSource(nameof(NullOrEmptyRecipients))] IEnumerable<EmailRecipient>? recipients)
        {
            EmailMessageRequest message = CreateValidEmailMessageRequest() with
            {
                Recipients = recipients
            };

            using HttpResponseMessage response = await SendEmailAsync(message);
            var expectedProblemDetails = new ValidationProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "One or more validation errors occurred.",
                Errors =
                {
                    { nameof(message.Recipients), new [] { $"The {nameof(message.Recipients)} field is required." } }
                }
            };

            await response.Should().HaveStatusCode(HttpStatusCode.BadRequest)
                .HaveJsonContentAsync(expectedProblemDetails, opts => opts.Excluding(x => x.Errors["traceId"]));
        }

        private static Faker<EmailRecipient> CreateEmailRecipientFaker() => new Faker<EmailRecipient>()
                .RuleFor(recipient => recipient.EmailAddress, random => random.Internet.Email())
                .RuleFor(recipient => recipient.Name, random => $"{random.Person.FirstName} {random.Person.LastName}");

        private static EmailMessageRequest CreateValidEmailMessageRequest()
        {
            var fakeRecipients = CreateEmailRecipientFaker();

            var fakeContent = new Faker<EmailContent>()
                .RuleFor(content => content.Value, fake => fake.Lorem.Paragraph())
                .RuleFor(content => content.ContainsHtml, fake => fake.Random.Bool());

            return new Faker<EmailMessageRequest>()
                .RuleFor(request => request.Recipients, fake => fakeRecipients.Generate(fake.Random.Int(1, 4)))
                .RuleFor(request => request.Subject, fake => fake.Lorem.Sentence())
                .RuleFor(request => request.Content, fake => fakeContent.Generate())
                .Generate();
        }

        private async Task<HttpResponseMessage> SendEmailAsync(EmailMessageRequest request)
        {
            using var client = _factory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            return await client.PostAsync("api/v1/emails", content);
        }
    }
}