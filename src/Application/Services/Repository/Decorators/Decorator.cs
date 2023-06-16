using BlueBrown.BigBola.Application.Entities;
using BlueBrown.BigBola.Application.Services.Metrics;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;

namespace BlueBrown.BigBola.Application.Services.Repository.Decorators
{
	public class RepositoryDecorator : IRepository
	{
		private readonly IRepository _repository;
		private readonly ILogger<RepositoryDecorator> _logger;
		private readonly IMetrics _metrics;

		public RepositoryDecorator(
			IRepository repository,
			ILogger<RepositoryDecorator> logger,
			IMetrics metrics)
		{
			_repository = repository;
			_logger = logger;
			_metrics = metrics;
		}

		public async Task<IReadOnlyCollection<WalletAction>> ReadWalletActions(Request request)
		{
			_logger.LogInformation("Request: [{0}]", JsonConvert.SerializeObject(request));

			//todo read from settings
			var format = "dd/MM/yyyy";

			if (!DateTime.TryParseExact(request.StartDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var _)
				&& !DateTime.TryParseExact(request.EndDate, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var _))
			{
				_logger.LogError("Invalid date format");

				throw new ValidationException("Invalid date format");
			}

			if (request.Rows <= 0)
			{
				_logger.LogError("rows must be positive integer");

				throw new ValidationException("rows must be positive integer");
			}

			if (request.Page <= 0)
			{
				_logger.LogError("page must be positive integer");

				throw new ValidationException("page must be positive integer");
			}

			request.UpdateStartDate();
			request.UpdateEndDate();

			_logger.LogInformation("Repository started reading from DB");

			var stopwatch = Stopwatch.StartNew();

			var result = await _repository.ReadWalletActions(request);

			stopwatch.Stop();

			//todo pass duration into scopped logging
			//todo pass count into scopped logging
			_logger.LogInformation("Repository finished reading from DB after [{0}]ms", stopwatch.ElapsedMilliseconds);

			var tags = new Dictionary<string, string>
			{
				{ "method", nameof(ReadWalletActions) },
				{ "startDate", request.StartDate },
				{ "endDate", request.EndDate },
				{ "rows", request.Rows.ToString() },
				{ "page", request.Page.ToString() }
			};

			_metrics.MeasureTime(stopwatch.ElapsedMilliseconds, tags);

			_metrics.MeasureGauge(result.Count, tags);

			return result;
		}
	}
}
