
using Microsoft.AspNetCore.Mvc;

namespace BulkPaymentServer.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("ping")]
    public IActionResult Ping()
    {

        return Ok("response");
    }
}