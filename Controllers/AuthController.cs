using Microsoft.AspNetCore.Mvc;

namespace Hackathon2024API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> Register()
    {
        return Ok();
    }
}