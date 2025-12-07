using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BulkPaymentServer.Application.Interfaces;
using BulkPaymentServer.Infrastructure.Services;

namespace BulkPaymentServer.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration["BlobStorageConnectionString"];
        services.AddSingleton(new BlobServiceClient(connectionString));

        services.AddScoped<IFileStorageService, AzureBlobStorageService>();

        services.AddScoped<ICsvParser, CsvParser>();
        return services;
    }
}