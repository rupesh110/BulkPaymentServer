using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Serilog;
using DotNetEnv;

using BulkPaymentServer.Application;
using BulkPaymentServer.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using BulkPaymentServer.Api.Middleware;

var dir = Directory.GetCurrentDirectory();

while (dir != null)
{
    var envPath = Path.Combine(dir, ".env");
    if (File.Exists(envPath))
    {
        Env.Load(envPath);
        break;
    }

    dir = Directory.GetParent(dir)?.FullName;
}


var builder = WebApplication.CreateBuilder(args);

//logging
builder.Logging.ClearProviders();
builder.Configuration.AddJsonFile("serilog.json", optional: false, reloadOnChange: true);


//Azure key vault
var keyVaultUrl = builder.Configuration["KeyVault:Url"];
Console.WriteLine($"KeyVaultUrl = {keyVaultUrl}");
if (
    Uri.TryCreate(keyVaultUrl, UriKind.Absolute, out var vaultUri) 
    && !builder.Environment.IsDevelopment()
   )
{
    builder.Configuration.AddAzureKeyVault(
        vaultUri,
        new DefaultAzureCredential());
}


// Serilog setup
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();




builder.Host.UseSerilog();

//services
builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Middleware
app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionMiddleware>();


//Swagger config-driven
var enableSwagger = app.Configuration.GetValue<bool>("Swagger:Enabled");

if (enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Endpoints
app.MapControllers();
app.MapGet("/", () => "Hello World! Test kubernetes");

app.Run();
