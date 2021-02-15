using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Mail.Model;
using SmtpSender.WebApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bogus.Extensions;
using FluentValidation.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SmtpSender.Infrastructure.SendGrid.Settings;

namespace SmtpSender.WebApi.Tests.PseudoIntegration.EmailsControllerTests
{
    class When_Sending_An_Email
    {
#pragma warning disable CS8618
        private CustomWebApplicationFactory _factory;
        private Mock<ISendGridClient> _sendGridMock;
        private EmailSettings _fakeEmailSettings;
#pragma warning restore

        [SetUp]
        public void SetUp()
        {
            _sendGridMock = new Mock<ISendGridClient>();
            _fakeEmailSettings = new Faker<EmailSettings>()
                .RuleFor(settings => settings.SendGridApiKey, fake => fake.Random.Guid().ToString())
                .RuleFor(settings => settings.FromAddress, fake => fake.Internet.Email())
                .RuleFor(settings => settings.FromName, fake => fake.Name.FullName().OrNull(fake, 0.5f))
                .Generate();

            _factory = new CustomWebApplicationFactory(services =>
            {
                services.ReplaceServiceWithMock(_sendGridMock);
                services.Configure<EmailSettings>(settings =>
                {
                    settings.FromAddress = _fakeEmailSettings.FromAddress;
                    settings.FromName = _fakeEmailSettings.FromName;
                    settings.SendGridApiKey = _fakeEmailSettings.SendGridApiKey;
                });
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
            EmailMessageRequest request = CreateValidEmailMessageRequest() with
            {
                Recipients = recipients
            };

            using HttpResponseMessage response = await SendEmailAsync(request);
            var expectedProblemDetails = new ValidationProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "One or more validation errors occurred.",
                Errors =
                {
                    { nameof(request.Recipients), new [] { $"The {nameof(request.Recipients)} field is required." } }
                }
            };

            await response.Should().HaveStatusCode(HttpStatusCode.BadRequest)
                .HaveValidationProblemDetailsAsync(expectedProblemDetails);
        }

        [Test]
        public async Task Then_Recipients_Cannot_Contain_Nulls()
        {
            IEnumerable<EmailRecipient> recipientsWithNull = new List<EmailRecipient>(CreateEmailRecipientFaker().Generate(2))
            {
                null
            };

            EmailMessageRequest request = CreateValidEmailMessageRequest() with
            {
                Recipients = recipientsWithNull
            };

            using HttpResponseMessage response = await SendEmailAsync(request);
            var expectedResponse = new ValidationProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "One or more validation errors occurred.",
                Errors =
                {
                    { $"{nameof(request.Recipients)}[{recipientsWithNull.Count() - 1}]", new[] { $"The {nameof(request.Recipients)} field cannot contain nulls." } }
                }
            };

            await response.Should().HaveStatusCode(HttpStatusCode.BadRequest)
                .HaveValidationProblemDetailsAsync(expectedResponse);
        }

        [Test]
        public async Task Then_Email_Content_Property_Cannot_Be_Null()
        {
            EmailMessageRequest request = CreateValidEmailMessageRequest() with
            {
                Content = null
            };

            using HttpResponseMessage response = await SendEmailAsync(request);
            var expectedProblemDetails = new ValidationProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "One or more validation errors occurred.",
                Errors =
                {
                    { nameof(request.Content), new [] { $"The {nameof(request.Content)} field is required." } }
                }
            };

            await response.Should().HaveStatusCode(HttpStatusCode.BadRequest)
                .HaveValidationProblemDetailsAsync(expectedProblemDetails);
        }

        [Test]
        public async Task Then_Recipients_Email_Address_Must_Be_Valid_Email_Address()
        {
            EmailMessageRequest request = CreateValidEmailMessageRequest() with
            {
                Recipients = new[]
                {
                    new EmailRecipient
                    {
                        Name = "Fake",
                        EmailAddress = "bad"
                    }
                }
            };

            using HttpResponseMessage response = await SendEmailAsync(request);
            var expectedProblemDetails = new ValidationProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "One or more validation errors occurred.",
                Errors =
                {
                    { $"{nameof(request.Recipients)}[0].{nameof(EmailRecipient.EmailAddress)}", new [] { $"'{nameof(EmailRecipient.EmailAddress).SplitPascalCase()}' is not a valid email address." } }
                }
            };

            await response.Should().HaveStatusCode(HttpStatusCode.BadRequest)
                .HaveValidationProblemDetailsAsync(expectedProblemDetails);
        }

        [Test]
        public async Task Then_Email_Is_Sent([Values] bool contentContainsHtml)
        {
            EmailMessageRequest request = CreateValidEmailMessageRequest();

            var expectedMessage = new SendGridMessage
            {
                Subject = request.Subject,
                From = new EmailAddress(_fakeEmailSettings.FromAddress, _fakeEmailSettings.FromName)
            };

            if (contentContainsHtml)
            {
                expectedMessage.HtmlContent = request.Content.Value;
            }
            else
            {
                expectedMessage.PlainTextContent = request.Content.Value;
            }

            expectedMessage.AddTos(request.Recipients
                .Select(recipient => new EmailAddress(recipient.EmailAddress, recipient.Name))
                .ToList());

            _sendGridMock.Setup(client => client.SendEmailAsync(
                It.Is(Equivalent.To(expectedMessage)),
                It.IsAny<CancellationToken>()
            )).ReturnsAsync(new Response(HttpStatusCode.OK, new StringContent(string.Empty), null))
                .Verifiable();

            using HttpResponseMessage response = await SendEmailAsync(request);

            response.Should().HaveStatusCode(HttpStatusCode.OK);
        }

        private static Faker<EmailRecipient> CreateEmailRecipientFaker() => new Faker<EmailRecipient>()
                .RuleFor(recipient => recipient.EmailAddress, random => random.Internet.Email())
                .RuleFor(recipient => recipient.Name, random => $"{random.Person.FirstName} {random.Person.LastName}");

        private static EmailMessageRequest CreateValidEmailMessageRequest(bool? contentContainsHtml = null)
        {
            var fakeRecipients = CreateEmailRecipientFaker();

            var fakeContent = new Faker<EmailContent>()
                .RuleFor(content => content.Value, fake => fake.Lorem.Paragraph())
                .RuleFor(content => content.ContainsHtml, fake => contentContainsHtml ?? fake.Random.Bool());

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