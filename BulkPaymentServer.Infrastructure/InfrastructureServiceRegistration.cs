using Microsoft.Extensions.DependencyInjection;
using BulkPaymentServer.Application.Interfaces;
using BulkPaymentServer.Infrastructure.Services;

namespace BulkPaymentServer.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<ICsvParser, CsvParser>();
        return services;
    }
}