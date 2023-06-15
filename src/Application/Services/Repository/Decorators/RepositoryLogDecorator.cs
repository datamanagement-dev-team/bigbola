using BlueBrown.BigBola.Application.Entities;
using BlueBrown.BigBola.Application.Services.Metrics;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Globalization;

namespace BlueBrown.BigBola.Application.Services.Repository.Decorators
{
    public class RepositoryLogDecorator : IRepository
    {
        private readonly IRepository _repository;
        private readonly ILogger<RepositoryLogDecorator> _logger;
        private readonly IMetrics _metrics;
        private readonly ISettings _settings;

        public RepositoryLogDecorator(IRepository repository,
            ILogger<RepositoryLogDecorator> logger,
            IMetrics metrics,
            ISettings settings)
        {
            _repository = repository;
            _logger = logger;
            _metrics = metrics;
            _settings = settings;
        }

        public async Task<IReadOnlyCollection<WalletAction>> ReadWalletActions(Request request)
        {
            _logger.LogInformation("Request: startDate : {0}, endDate : {1}, rows : {2}, page : {3}",
                request.startDate,
                request.endDate,
                request.rows,
                request.page);

            DateTime date;
            if (!DateTime.TryParseExact(request.startDate, _settings.ValidRequestDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
                && !DateTime.TryParseExact(request.endDate, _settings.ValidRequestDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                _logger.LogError("Invalid date format");

                throw new RequestValidationException("Invalid date format");
            }

            if (request.rows <= 0)
            {
                _logger.LogError("rows must be positive integer");

                throw new RequestValidationException("rows must be positive integer");
            }

            if (request.page <= 0)
            {
                _logger.LogError("page must be positive integer");

                throw new RequestValidationException("page must be positive integer");
            }   

            _logger.LogInformation("Repository started reading from DB");

            request.UpdateStartDate();
            request.UpdateEndDate();

            var stopwatch = Stopwatch.StartNew();

            var result = await _repository.ReadWalletActions(request);

            stopwatch.Stop();

            _logger.LogInformation("Repository finished reading from DB after [{0}]ms", stopwatch.ElapsedMilliseconds);

            Dictionary<string, string> tags = new Dictionary<string, string>
            {
                { "method", "ReadWalletActions" },
                { "startDate", request.startDate },
                { "endDate", request.endDate },
                { "rows", request.rows.ToString() },
                { "page", request.page.ToString() }
            };

            _metrics.MeasureTime(stopwatch.ElapsedMilliseconds, tags);

            _metrics.MeasureGauge(result.Count, tags);

            return result;
        }
    }
}
