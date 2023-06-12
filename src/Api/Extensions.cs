using BlueBrown.BigBola.Application.Services.Repository;
using BlueBrown.BigBola.Infrastructure.Services.Repository;
using Microsoft.Extensions.DependencyInjection;
using NJsonSchema;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Net.Mime;

namespace BlueBrown.BigBola.Api
{
    internal static class Extensions
    {
        internal static void Configure(this IServiceCollection services)
        {
            services.RegisterOpenApi();

            services.AddSingleton<ApiKeyAuthorizationFilter>();
            services.AddScoped<IRepository, Repository>();

            services
                .AddControllers()
                .AddJsonOptions(_options =>
                {
                    _options.JsonSerializerOptions.WriteIndented = true;
                });
        }

        internal static void Configure(this IApplicationBuilder builder)
        {
            builder.UseRouting();

            builder.UseOpenApi();

            builder.UseEndpoints(_builder =>
            {
                _builder.MapControllers();
            });
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

                _settings.AddSecurity("ApiKeyAuth", Enumerable.Empty<string>(), new OpenApiSecurityScheme { 
                    Type = OpenApiSecuritySchemeType.ApiKey, 
                    In = OpenApiSecurityApiKeyLocation.Header, 
                    Name = "Operation-API-Key",
                    
                });

                _settings.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("ApiKeyAuth"));

            });
        }

        private static void UseOpenApi(this IApplicationBuilder builder)
        {

            builder.Use(async (_httpContext, _nextRequestDelegate) =>
            {
                if (_httpContext.Request.Path.Value == "/")
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
