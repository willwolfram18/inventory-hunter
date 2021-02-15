using FluentAssertions;
using FluentAssertions.Equivalency;
using FluentAssertions.Primitives;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SmtpSender.WebApi.Tests.PseudoIntegration
{
    public static class HttpResponseMessageExtensions
    {
        public static HttpResponseMessageAssertions Should(this HttpResponseMessage subject) =>
            new HttpResponseMessageAssertions(subject);
    }

    public class HttpResponseMessageAssertions : ReferenceTypeAssertions<HttpResponseMessage, HttpResponseMessageAssertions>
    {
        public HttpResponseMessageAssertions(HttpResponseMessage subject)
        {
            Subject = subject;
        }

        /// <inheritdoc />
        protected override string Identifier => "HttpResponseMessage";

        public HttpResponseMessageAssertions HaveStatusCode(HttpStatusCode expected, string because = null,
            params object[] becauseArgs)
        {
            Subject.StatusCode.Should().Be(expected, because, becauseArgs);

            return this;
        }

        public Task<HttpResponseMessageAssertions> HaveValidationProblemDetailsAsync(ValidationProblemDetails actual,
            string because = null, params object[] becauseArgs) =>
            HaveJsonContentAsync(actual, opts => opts.Excluding(x => x.Extensions["traceId"]), because, becauseArgs);

        public Task<HttpResponseMessageAssertions> HaveJsonContentAsync<T>(T response, string because = null,
            params object[] becauseArgs) =>
        HaveJsonContentAsync(response, _ => _, because, becauseArgs);

        public async Task<HttpResponseMessageAssertions> HaveJsonContentAsync<T>(T actual,
            Func<EquivalencyAssertionOptions<T>, EquivalencyAssertionOptions<T>> config, string because = null,
            params object[] becauseArgs)
        {
            var actualResponse = JsonConvert.DeserializeObject<T>(await Subject.Content.ReadAsStringAsync());

            actualResponse.Should().BeEquivalentTo(actual, config, because, becauseArgs);

            return this;
        }
    }
}
