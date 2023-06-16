using BlueBrown.BigBola.Api;
using BlueBrown.BigBola.Infrastructure;

var webApplicationBuilder = WebApplication.CreateBuilder(args);

var environmentName = webApplicationBuilder.Environment.EnvironmentName;

webApplicationBuilder.Configuration.Configure(environmentName);

webApplicationBuilder.Logging.Configure();

webApplicationBuilder.Host.ConfigureMetrics(webApplicationBuilder.Configuration);

webApplicationBuilder.Services.Configure(webApplicationBuilder.Configuration);

var webApplication = webApplicationBuilder.Build();

webApplication.Configure();

await webApplication.RunAsync();
