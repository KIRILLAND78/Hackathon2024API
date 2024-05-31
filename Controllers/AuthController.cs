using Hackathon2024API.Data.Settings;
using Hackathon2024API.DTO.User;
using Hackathon2024API.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;


namespace Hackathon2024API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;

	public AuthController(IAuthService authService)
	{
		_authService = authService;
	}

	[HttpPost("register")]
	public async Task<ActionResult<UserDto>> Register(RegisterUserDto dto)
	{
		var response = await _authService.Register(dto);
		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}
	
	
	[HttpPost("login")]
	public async Task<ActionResult<Token>> Login(LoginUserDto loginUserDto)
	{
		var response = await _authService.Login(loginUserDto);
		
		if (response.IsSuccess)
		{
			return Ok(response);
		}

		return BadRequest(response);
	}
}
