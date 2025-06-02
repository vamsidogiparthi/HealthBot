using Microsoft.AspNetCore.Mvc;

namespace HealthCareAgent.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ChatController() : ControllerBase
{
    public IActionResult Get()
    {
        return Ok("Chat API is running");
    }
}
