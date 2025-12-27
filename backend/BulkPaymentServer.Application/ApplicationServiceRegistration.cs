using Microsoft.Extensions.DependencyInjection;
using BulkPaymentServer.Application.Interfaces;
using BulkPaymentServer.Application.Services;

namespace BulkPaymentServer.Application;


public static class ApplicationServiceRegistration
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services)
	{
		services.AddScoped<ICsvProcessor, CsvProcessor>();
        services.AddScoped<IUploadService, UploadService>();
        services.AddScoped<UploadEventPublisher>();


        return services;
	}
}