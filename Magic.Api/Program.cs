using Magic.Api;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var environment = builder.Environment;
var startup = new Startup(configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, environment);

await app.RunAsync();