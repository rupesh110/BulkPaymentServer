using Azure.Identity;
using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Serilog;
using BulkPaymentServer.Application;
using BulkPaymentServer.Infrastructure;


var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("serilog.json", optional: false, reloadOnChange: true);

var keyVaultUrl = builder.Configuration["KeyVaultUrl"];
builder.Configuration.AddAzureKeyVault(new Uri(keyVaultUrl), new DefaultAzureCredential());

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

if (app.Environment.IsDevelopment())
{
   app.UseSwagger();
   app.UseSwaggerUI();
}

app.MapControllers();
app.MapGet("/", () => "Hello World! Test");

app.Run();
