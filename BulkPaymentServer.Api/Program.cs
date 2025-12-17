using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Serilog;

using BulkPaymentServer.Application;
using BulkPaymentServer.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using BulkPaymentServer.Api.Middleware;


var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();

builder.Configuration.AddJsonFile("serilog.json", optional: false, reloadOnChange: true);

var keyVaultUrl = builder.Configuration["KeyVaultUrl"];

if (
    Uri.TryCreate(keyVaultUrl, UriKind.Absolute, out var vaultUri) 
    && !builder.Environment.IsDevelopment()
   )
{
    builder.Configuration.AddAzureKeyVault(
        vaultUri,
        new DefaultAzureCredential());
}

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();


builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
}

app.MapControllers();
app.MapGet("/", () => "Hello World! Test");

app.Run();
