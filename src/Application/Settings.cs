namespace BlueBrown.BigBola.Application
{
    public interface ISettings
    {
        string ReportingConnectionString { get; }
        string MetricsUrl { get; }
        string HealthChecksUrl { get; }
    }
}
