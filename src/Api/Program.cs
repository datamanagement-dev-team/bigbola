var webApplicationBuilder = WebApplication.CreateBuilder(args);

var webApplication = webApplicationBuilder.Build();

await webApplication.RunAsync();
