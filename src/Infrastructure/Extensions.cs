using App.Metrics.Formatters.Prometheus;
using BlueBrown.BigBola.Application;
using BlueBrown.BigBola.Application.Services.Metrics;
using BlueBrown.BigBola.Application.Services.Repository.Decorators;
using BlueBrown.BigBola.Application.Services.Repository;
using BlueBrown.BigBola.Infrastructure.Services.Metrics;
using BlueBrown.BigBola.Infrastructure.Services.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using BlueBrown.Common.Consul.Modules;
using BlueBrown.Common.Consul.NetConfiguration.Extensions;
using BlueBrown.Common.Vault.Modules.Configurations;
using BlueBrown.Common.Vault.ReadSecret;
using BlueBrown.Common.Vault.Modules;

namespace BlueBrown.BigBola.Infrastructure
{
    public static class Extensions
    {
        public static void RegisterSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var bbreportingConnectionSettings = configuration.GetSection(nameof(BBReportingConnectionSettings)).Get<BBReportingConnectionSettings>()!;

            Infrastructure.Settings settings = configuration.GetSection(nameof(Infrastructure.Settings)).Get<Infrastructure.Settings>()!;

            var connectionStringTemplate = settings.ConnectionStringTemplate;

            var connectionString = string.Format(
                connectionStringTemplate,
                settings.ReportingDatabaseServerName,
                settings.ReportingDatabaseName,
            bbreportingConnectionSettings.Username,
                bbreportingConnectionSettings.Password);

            settings.SetReportingConnectionString(connectionString);

            services.AddSingleton<ISettings>(settings);
        }

        public static void RegisterRepository(this IServiceCollection services)
        {
            services.AddScoped<Repository>();

            services.AddScoped<IRepository>(provider =>
                new RepositoryLogDecorator(provider.GetRequiredService<Repository>(),
                provider.GetRequiredService<ILoggerFactory>().CreateLogger<RepositoryLogDecorator>(),
                provider.GetRequiredService<IMetrics>(),
                provider.GetRequiredService<ISettings>()));
        }

        public static void RegisterMetrics(this IServiceCollection services)
        {
            services.AddScoped<IMetrics, Metrics>();

            services.AddMetrics(_builder =>
            {
                _builder.Configuration.Configure(_options =>
                {
                    _options.ContextualTags.Clear();

                    _options.GlobalTags.Clear();
                });
            });

            services.AddMetricsEndpoints(_options =>
            {
                _options.MetricsEndpointOutputFormatter = new MetricsPrometheusTextOutputFormatter();
            });

            services.AddAppMetricsHealthPublishing();
        }

        public static void ConfigureMetrics(this IHostBuilder builder, IConfiguration configuration)
        {
            builder.ConfigureMetrics(_builder =>
            {
                _builder.Configuration.Configure(_options =>
                {
                    _options.ContextualTags.Clear();

                    _options.GlobalTags.Clear();
                });
            });

            builder.UseMetricsEndpoints(_options =>
            {
                _options.EnvironmentInfoEndpointEnabled = false;

                _options.MetricsEndpointEnabled = true;

                _options.MetricsTextEndpointEnabled = false;
            });

            builder.ConfigureAppMetricsHostingConfiguration(_options =>
            {
                var settings = new Infrastructure.Settings();

                configuration.Bind(nameof(Infrastructure.Settings), settings);

                _options.MetricsEndpoint = settings.MetricsUrl;
            });
        }

        public static void AddConsulVault(this IConfigurationBuilder builder)
        {
            var configuration = builder.Build();

            var settings = configuration.GetSection(nameof(Settings)).Get<Settings>()!;

            VaultConfigSecretEvaluator.roleId = settings.VaultRoleId;
            VaultConfigSecretEvaluator.secretId = settings.VaultSecretId;
            VaultConfigSecretEvaluator.vaultServerUriWithPort = settings.VaultUrl;

            builder.AddConsulForConfiguration(
                mainKey: settings.ConsulMainKey,
                key: settings.ConsulKey,
                options: _source =>
                {
                    _source.Optional = true;

                    _source.ReloadOnChange = true;

                    _source.ConsulConfigurationOptions = _configuration =>
                    {
                        _configuration.Address = new Uri(settings.ConsulUrl);

                        _configuration.Token = settings.ConsulToken;
                    };
                },
                VaultConfigSecretEvaluator.EvaluateSecretsWithVault);
        }

        public static void RegisterConsulVault(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection(nameof(Settings)).Get<Settings>()!;

            var vaultModuleConfiguration = new VaultModuleConfiguration
            {
                AbsoluteExpirationMinutes = settings.ConsulAbsoluteExpirationInMinutes,
                VaultServerUriWithPort = settings.VaultUrl
            };

            services
                .RegisterConsulModule(
                    majorVersion: settings.ConsulMajorVersion,
                    refreshConfigurationInSeconds: settings.ConsulRefreshConfigurationInSeconds,
                    consulUrl: settings.ConsulUrl,
                    consulToken: settings.ConsulToken,
                    prefixPath: settings.ConsulPrefixPath)
                .AddSingleton(vaultModuleConfiguration)
                .RegisterVaultModule(
                    roleId: settings.VaultRoleId,
                    secretId: settings.VaultSecretId);
        }

        public static void AddNLog(this ILoggingBuilder builder)
        {
            builder.AddNLog("nlog.config");
        }

        public static void ShutdownLogging()
        {
            NLog.LogManager.Shutdown();
        }

        internal record Settings
        {
            public string VaultRoleId { get; init; } = string.Empty;
            public string VaultSecretId { get; init; } = string.Empty;
            public string VaultUrl { get; init; } = string.Empty;
            public string ConsulMainKey { get; init; } = string.Empty;
            public string ConsulKey { get; init; } = string.Empty;
            public string ConsulUrl { get; init; } = string.Empty;
            public string ConsulToken { get; init; } = string.Empty;
            public int ConsulAbsoluteExpirationInMinutes { get; init; }
            public int ConsulMajorVersion { get; init; }
            public int ConsulRefreshConfigurationInSeconds { get; init; }
            public string ConsulPrefixPath { get; init; } = string.Empty;
        }

        internal abstract record ConnectionStringSettings
        {
            public string Username { get; init; } = string.Empty;
            public string Password { get; init; } = string.Empty;
        }

        internal record BBReportingConnectionSettings : ConnectionStringSettings { }
    }
}
