using System.Security.Claims;
using Hackathon2024API.Data;
using Hackathon2024API.DTO.User;
using Hackathon2024API.Data.Models;
using Hackathon2024API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Hackathon2024API.Data.DTO;

namespace Hackathon2024API.Controllers;

[ApiController]
[Route("/api/[controller]")]

public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;

    public AdminController(IAdminService adminService, UserManager<User> userManager, ApplicationDbContext db)
    {
        _adminService = adminService;
        _userManager = userManager;
        _context = db;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateUserLimitSettings(LimitSettings limitSettings)
    {
        await _adminService.CreateChanges(limitSettings);

        return Ok();
    }
    [HttpGet("Index")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers([FromQuery]PaginationDTO pg)
    {
        return Ok(_context.Users.Skip(pg.Count*pg.Page).Take(pg.Count).ToList());

    }
}