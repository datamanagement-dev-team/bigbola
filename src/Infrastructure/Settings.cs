using BlueBrown.BigBola.Application;

namespace BlueBrown.BigBola.Infrastructure
{
    public record Settings : ISettings
    {
        public string ConnectionStringTemplate { get; init; } = string.Empty;
        public string ReportingDatabaseServerName { get; init; } = string.Empty;
        public string ReportingDatabaseName { get; init; } = string.Empty;
        public string ReportingConnectionString { get; private set; } = string.Empty;
        public string MetricsUrl { get; init; } = string.Empty;
        public string HealthChecksUrl { get; init; } = string.Empty;

        public void SetReportingConnectionString(string connectionString)
        {
            ReportingConnectionString = connectionString;
        }
    }
}
