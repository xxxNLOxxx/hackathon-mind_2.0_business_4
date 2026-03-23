using Microsoft.AspNetCore.Mvc;

namespace HackathonApp.Controllers;

[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("API работает");
    }
}