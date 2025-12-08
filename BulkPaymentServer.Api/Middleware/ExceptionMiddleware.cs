using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace BulkPaymentServer.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            // Log full details only on server side
            _logger.LogError(ex, "Unhandled exception occurred");

            // Return safe error response to client
            await WriteSafeErrorResponseAsync(context);
        }
    }

    private static Task WriteSafeErrorResponseAsync(HttpContext context)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var response = new
        {
            message = "An unexpected error occurred. Please try again later."
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}
