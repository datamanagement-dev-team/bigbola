namespace BlueBrown.BigBola.Application.Services.Metrics
{
    public interface IMetrics
    {
        void MeasureGauge(int value, IReadOnlyDictionary<string, string> tags);
        void MeasureTime(long time, IReadOnlyDictionary<string, string> tags);
    }
}
