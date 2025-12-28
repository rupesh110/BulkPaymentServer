using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

using BulkPaymentServer.Application.Interfaces;
using BulkPaymentServer.Infrastructure.Services;
using BulkPaymentServer.Infrastructure.Persistence;
using BulkPaymentServer.Infrastructure.Repositories;
using BulkPaymentServer.Infrastructure.Kafka;

namespace BulkPaymentServer.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        //Blob Storage
        string connectionString = configuration["BlobStorage:ConnectionString"];
        if (string.IsNullOrWhiteSpace(connectionString)) {
            throw new InvalidOperationException(
                "BlobStorage: Connection string is missing"
                );
        }
        services.AddSingleton(new BlobServiceClient(connectionString));

        //EF Core DbContext
        string dbConnectionString = configuration["Sql:ConnectionString"]; 


        //Kafka 
        services.AddSingleton<IKafkaProducer, KafkaProducer>();

        services.AddDbContext<BulkPaymentDbContext>(options =>
            options.UseSqlServer(dbConnectionString));


        services.AddScoped<IFileStorageService, AzureBlobStorageService>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();


        services.AddScoped<ICsvParser, CsvParser>();
        return services;
    }
}