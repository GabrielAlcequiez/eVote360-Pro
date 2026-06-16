using eVote360_Pro.Core.Application.Interfaces;
using eVote360_Pro.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eVote360_Pro.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureLayer(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Enlaza la sección "EmailSettings" de appsettings.json
            // con el POCO EmailSettings usando el patrón Options.
            services.Configure<EmailSettings>(opts =>
                configuration.GetSection("EmailSettings").Bind(opts));

            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IOcrService, OcrService>();
        }
    }
}
