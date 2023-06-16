using BlueBrown.BigBola.Application;
using BlueBrown.BigBola.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using NJsonSchema;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Net.Mime;

namespace BlueBrown.BigBola.Api
{
	internal static class Extensions
	{
		internal static void Configure(this IConfigurationBuilder builder, string environmentName)
		{
			builder.Sources.Clear();

			builder.AddJsonFile(path: "appsettings.json", optional: false);

			builder.AddConsulVault();

			builder.AddJsonFile(path: $"appsettings.{environmentName}.json", optional: true);

			builder.SetBasePath(Directory.GetCurrentDirectory());

			builder.SetFileLoadExceptionHandler(_context =>
			{
				throw _context.Exception;
			});
		}

		internal static void Configure(this IServiceCollection services, IConfiguration configuration)
		{
			services.RegisterSettings(configuration);

			services.RegisterConsulVault(configuration);

			services.RegisterMetrics();

			services.RegisterOpenApi();

			services.AddSingleton<ApiKeyAuthorizationFilter>();

			services.RegisterRepository();

			services
				.AddControllers()
				.AddJsonOptions(_options =>
				{
					_options.JsonSerializerOptions.WriteIndented = true;
				});

			services.RegisterHealthChecks();

			services.AddTransient<ExceptionHandlingMiddleware>();
		}

		internal static void Configure(this ILoggingBuilder builder)
		{
			builder.ClearProviders();

			builder.SetMinimumLevel(LogLevel.Trace);

			builder.AddNLog();
		}

		internal static void Configure(this IApplicationBuilder builder)
		{
			builder.UseRouting();

			builder.UseOpenApi();

			//todo add middleware as transient
			builder.UseMiddleware<ExceptionHandlingMiddleware>();

			builder.UseEndpoints(_builder =>
			{
				_builder.MapControllers();

				var settings = builder.ApplicationServices.GetRequiredService<ISettings>();

				_builder.MapHealthChecks(settings.HealthChecksUrl, new HealthCheckOptions
				{
					Predicate = _ => true,
					ResponseWriter = async (_httpContext, _healthReport) =>
					{
						var settings = new JsonSerializerSettings
						{
							Converters = new List<JsonConverter>
							{
								new StringEnumConverter()
							},
							Formatting = Formatting.Indented
						};

						var response = JsonConvert.SerializeObject(_healthReport, settings);


						await _httpContext.Response.WriteAsync(response);
					},
				});
			});
		}

		private static void RegisterHealthChecks(this IServiceCollection services)
		{
			using var serviceProvider = services.BuildServiceProvider();

			var settings = serviceProvider.GetRequiredService<ISettings>();

			services
				.AddHealthChecks()
				.AddSqlServer(
					connectionString: settings.ReportingConnectionString,
					name: "BBReporting",
					failureStatus: HealthStatus.Unhealthy);
		}

		private static void RegisterOpenApi(this IServiceCollection services)
		{
			services.AddSwaggerDocument(_settings =>
			{
				_settings.SchemaType = SchemaType.OpenApi3;

				_settings.DocumentName = "api documentation";

				_settings.PostProcess = _openApiDocument =>
				{
					_openApiDocument.Info = new OpenApiInfo
					{
						Description = "api documentaion",
						Title = "api",
						Version = "1.0.0"
					};

					_openApiDocument.Consumes = new string[]
					{
						MediaTypeNames.Application.Json
					};

					_openApiDocument.Produces = new string[]
					{
						MediaTypeNames.Application.Json
					};
				};

				var securityDefinitionName = "ApiKeyAuthentication";

				_settings.AddSecurity(
					name: securityDefinitionName,
					globalScopeNames: Enumerable.Empty<string>(),
					swaggerSecurityScheme: new OpenApiSecurityScheme
					{
						Type = OpenApiSecuritySchemeType.ApiKey,
						In = OpenApiSecurityApiKeyLocation.Header,
						//todo get from settings
						Name = "Operation-API-Key",

					});

				_settings.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor(securityDefinitionName));

			});
		}

		private static void UseOpenApi(this IApplicationBuilder builder)
		{
			builder.Use(async (_httpContext, _nextRequestDelegate) =>
			{
				if (_httpContext.Request.Path.Value == "/")
					//todo get from settings
					_httpContext.Request.Path = "/api/documentation";

				await _nextRequestDelegate();
			});

			NSwagApplicationBuilderExtensions.UseOpenApi(builder);

			builder.UseSwaggerUi3(_swaggerUi3Settings =>
			{
				_swaggerUi3Settings.Path = "/api/documentation";

				_swaggerUi3Settings.DocumentTitle = "api documentation";
			});
		}
	}
}
