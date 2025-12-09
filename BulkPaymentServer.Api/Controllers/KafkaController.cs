using BulkPaymentServer.Application.Interfaces;
using BulkPaymentServer.Infrastructure.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace BulkPaymentServer.Api.Controllers;

[ApiController]
[Route("api/kafka")]
public class KafkaController : ControllerBase
{
    private readonly IKafkaProducer _producer;

    public KafkaController(IKafkaProducer producer)
    {
        _producer = producer;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendMessage(string key, [FromBody] string message)
    {   
        await _producer.SendMessageAsync(key, message);
        return Ok("Message sent to Kafka topic.");
    }
}