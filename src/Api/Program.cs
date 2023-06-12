using BlueBrown.BigBola.Api;

var webApplicationBuilder = WebApplication.CreateBuilder(args);

webApplicationBuilder.Services.Configure();

var webApplication = webApplicationBuilder.Build();

webApplication.Configure();

await webApplication.RunAsync();
