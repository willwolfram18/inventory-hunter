using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace SmtpSender.WebApi.Tests.PseudoIntegration
{
    internal class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        /// <summary>
        /// A service configuration delegate that makes no modifications to
        /// the <see cref="IServiceCollection" /> instance.
        /// </summary>
        private static readonly Action<IServiceCollection> NoServiceModifications = _ => {};

        private readonly Action<IServiceCollection> _configureServices;

        public CustomWebApplicationFactory() : this(NoServiceModifications)
        {
        }

        public CustomWebApplicationFactory(Action<IServiceCollection> configureServices)
        {
            _configureServices = configureServices;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(_configureServices);

            base.ConfigureWebHost(builder);
        }
    }
}