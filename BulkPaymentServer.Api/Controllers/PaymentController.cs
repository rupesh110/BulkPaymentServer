
using Microsoft.AspNetCore.Mvc;
using BulkPaymentServer.Application.Interfaces;

namespace BulkPaymentServer.Api.Controllers;

[ApiController]
[Route("api")]
public class PaymentController : ControllerBase
{
    private readonly IUploadService _uploadService;
    private readonly ILogger<PaymentController> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public PaymentController(
        IUploadService uploadService,
        ILogger<PaymentController> logger,
        IServiceScopeFactory scopeFactory)
    {
        _uploadService = uploadService;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    [HttpGet("payment")]
    public IActionResult GetPayment()
    {
        return Ok("Payment endpoint is working.");
    }

    [HttpPost("upload")]
    public IActionResult Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var userId = "User123";

        using var ms = new MemoryStream();
        file.CopyTo(ms);
        var fileBytes = ms.ToArray();

        _ = Task.Run(async () =>
        {
            using var scope = _scopeFactory.CreateScope();

            var uploadService =
                scope.ServiceProvider.GetRequiredService<IUploadService>();

            try
            {
                using var stream = new MemoryStream(fileBytes);
                await uploadService.UploadFileAsync(
                    userId,
                    stream,
                    file.FileName
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing upload {File}", file.FileName);
            }
        });

        return Accepted(new
        {
            message = "File upload accepted for processing"
        });
    }

}