
using Microsoft.AspNetCore.Mvc;
using BulkPaymentServer.Application.Interfaces;

namespace BulkPaymentServer.Api.Controllers;

[ApiController]
[Route("api")]
public class PaymentController : ControllerBase
{
    private readonly IUploadService _uploadService;
    private readonly ILogger<PaymentController> _logger;


    public PaymentController(IUploadService uploadService, ILogger<PaymentController> logger)
    {
        _uploadService = uploadService;
        _logger = logger;
    }

    [HttpGet("payment")]
    public IActionResult GetPayment()
    {
        return Ok("Payment endpoint is working.");
    }

    [HttpPost("upload")]
    public async Task<IActionResult> Upload(IFormFile file)
    {

        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var userId = "User123"; //TODO: Get from auth later

        using var stream = file.OpenReadStream();
        var result = await _uploadService.UploadFileAsync(
            userId,
            stream,
            file.FileName
        );
        return Ok(result);
    }
}