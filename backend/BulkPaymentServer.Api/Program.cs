using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Serilog;


using BulkPaymentServer.Application;
using BulkPaymentServer.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using BulkPaymentServer.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// --------------------
// Configuration
// --------------------
builder.Configuration.AddJsonFile(
    "serilog.json",
    optional: false,
    reloadOnChange: true);

// Azure Key Vault
var keyVaultEnabled = builder.Configuration.GetValue<bool>("KeyVault:Enabled");
if (keyVaultEnabled)
{
    var keyVaultUrl = builder.Configuration["KeyVault:Url"];
    if (Uri.TryCreate(keyVaultUrl, UriKind.Absolute, out var vaultUri))
    {
        builder.Configuration.AddAzureKeyVault(
            vaultUri,
            new DefaultAzureCredential());
    }
}

// --------------------
// Logging
// --------------------
builder.Logging.ClearProviders();

builder.Host.UseSerilog((ctx, lc) =>
{
    lc.ReadFrom.Configuration(ctx.Configuration)
      .Enrich.FromLogContext()
      .WriteTo.Console();
});

// --------------------
// Services
// --------------------
builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --------------------
// App
// --------------------
var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionMiddleware>();

if (app.Configuration.GetValue<bool>("Swagger:Enabled"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapGet("/", () => "Hello World! Test");

app.Run();
