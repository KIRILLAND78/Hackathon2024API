using System.Security.Claims;
using Hackathon2024API.DTO.User;
using Hackathon2024API.Models;
using Hackathon2024API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hackathon2024API.Controllers;

[ApiController]
[Route("/api/[controller]")]

public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly UserManager<User> _userManager;

    public AdminController(IAdminService adminService, UserManager<User> userManager)
    {
        _adminService = adminService;
        _userManager = userManager;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUserLimitSettings(LimitSettings limitSettings)
    {
        await _adminService.CreateChanges(limitSettings);

        return Ok("успех");
        
    }
}