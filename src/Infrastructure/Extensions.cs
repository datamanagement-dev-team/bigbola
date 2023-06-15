using BlueBrown.BigBola.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using static BlueBrown.BigBola.Infrastructure.Extensions;

namespace BlueBrown.BigBola.Infrastructure
{
    public static class Extensions
    {
        public static void RegisterSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var bbreportingConnectionSettings = configuration.GetSection(nameof(BBReportingConnectionSettings)).Get<BBReportingConnectionSettings>()!;

            var validRequestDateFormatSettings = configuration.GetSection(nameof(ValidRequestDateFormatSettings)).Get<ValidRequestDateFormatSettings>()!;

            var settings = configuration.GetSection(nameof(Settings)).Get<Settings>()!;

            var connectionStringTemplate = settings.ConnectionStringTemplate;

            var connectionString = string.Format(
                connectionStringTemplate,
                settings.ReportingDatabaseServerName,
                settings.ReportingDatabaseName,
            bbreportingConnectionSettings.Username,
                bbreportingConnectionSettings.Password);

            settings.SetReportingConnectionString(connectionString);

            settings.SetValidRequestDateFormat(validRequestDateFormatSettings.ValidRequestDateFormat);

            services.AddSingleton<ISettings>(settings);
        }

        public static void AddNLog(this ILoggingBuilder builder)
        {
            builder.AddNLog("nlog.config");
        }

        public static void ShutdownLogging()
        {
            NLog.LogManager.Shutdown();
        }

        internal abstract record ConnectionStringSettings
        {
            public string Username { get; init; } = string.Empty;
            public string Password { get; init; } = string.Empty;
        }

        internal record BBReportingConnectionSettings : ConnectionStringSettings { }
        internal record ValidRequestDateFormatSettings
        {
            public string ValidRequestDateFormat { get; init; } = string.Empty;
        }
    }
}
