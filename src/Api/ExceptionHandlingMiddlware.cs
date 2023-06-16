using BlueBrown.BigBola.Application;

namespace BlueBrown.BigBola.Api
{
	//todo move to a Middlewares folder
	internal class ExceptionHandlingMiddleware : IMiddleware
	{
		private readonly ILogger<ExceptionHandlingMiddleware> _logger;

		public ExceptionHandlingMiddleware(
			ILogger<ExceptionHandlingMiddleware> logger)
		{
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{

			try
			{
				await next(context);
			}
			catch (AuthenticationException exception)
			{
				_logger.LogError(exception, string.Empty);

				context.Response.StatusCode = StatusCodes.Status401Unauthorized;

				//todo what to return
				await context.Response.WriteAsJsonAsync(exception.Message);
			}
			catch (ValidationException ex)
			{
				_logger.LogError(ex, string.Empty);

				context.Response.StatusCode = StatusCodes.Status400BadRequest;

				//todo what to return
				await context.Response.WriteAsJsonAsync(ex.Message);
			}
			catch (Exception exception)
			{
				_logger.LogError(exception, string.Empty);

				context.Response.StatusCode = StatusCodes.Status500InternalServerError;

				//todo what to return
				await context.Response.WriteAsJsonAsync(exception.Message);
			}
		}
	}
}
