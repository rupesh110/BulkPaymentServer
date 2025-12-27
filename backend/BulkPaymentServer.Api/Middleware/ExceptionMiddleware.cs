using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BulkPaymentServer.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger,
        IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Generate a correlation ID
            var errorId = Guid.NewGuid().ToString();

            // Log EVERYTHING internally
            _logger.LogError(ex, "Unhandled exception | ErrorId: {ErrorId}", errorId);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            // Response sent to client (SAFE)
            await context.Response.WriteAsJsonAsync(new
            {
                error = "An unexpected error occurred.",
                errorId
            });
        }
    }
}
