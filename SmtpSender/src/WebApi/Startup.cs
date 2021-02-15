using SmtpSender.Domain;
using SmtpSender.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SmtpSender.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddVersionedApiExplorer(versionedApiExplorerSettings =>
            {
                versionedApiExplorerSettings.GroupNameFormat = "'v'VVV";
                versionedApiExplorerSettings.AssumeDefaultVersionWhenUnspecified = true;
                versionedApiExplorerSettings.SubstituteApiVersionInUrl = true;
            }).AddApiVersioning(apiVersioningSettings =>
            {
                apiVersioningSettings.DefaultApiVersion = new ApiVersion(1, 0);
                apiVersioningSettings.AssumeDefaultVersionWhenUnspecified = true;
                apiVersioningSettings.ReportApiVersions = true;
            });

            services.AddSwaggerGen();
            services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails =
                    (_, _) => Configuration.GetValue<bool>("Debug:IncludeExceptionDetails");
            });

            services.AddEmailService().AddSendGridEmailSender("TODO");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseProblemDetails();

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.DocumentTitle = "SmtpSender.WebApi";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "SmtpSender.WebApi");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
