using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace SmtpSender.WebApi.Tests.PseudoIntegration
{
    public class SampleTest
    {
        private CustomWebApplicationFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new CustomWebApplicationFactory();
        }

        [TearDown]
        public void TearDown()
        {
            // It's important to dispose a WebApplicationFactory
            // so that it properly shuts down the in memory server after
            // each test. Otherwise it can leak memory and processes.
            _factory.Dispose();
        }

        [Test]
        public async Task SwaggerPageLoads()
        {
            HttpClient client = _factory.CreateClient();

            using HttpResponseMessage response = await client.GetAsync("swagger/index.html");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}