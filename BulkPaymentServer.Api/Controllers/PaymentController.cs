
using Microsoft.AspNetCore.Mvc;
using BulkPaymentServer.Application.Interfaces;

namespace BulkPaymentServer.Api.Controllers;

[ApiController]
[Route("api")]
public class PaymentController : ControllerBase
{
    private readonly ICsvProcessor _csvProcessor;
    private readonly ILogger<PaymentController> _logger;


    public PaymentController(ICsvProcessor csvProcessor, ILogger<PaymentController> logger)
    {
       _csvProcessor = csvProcessor;
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

        using var reader = new StreamReader(file.OpenReadStream());
        var csvContent = await reader.ReadToEndAsync();

        try
        {
            var payments = await _csvProcessor.ProcessCsvAsync(csvContent);
            return Ok(new
            {
                message = "File uploaded successfully",
                length = csvContent.Length
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing CSV file.");
            return StatusCode(500, "Error Processgin csv file.");
        }
    }
}