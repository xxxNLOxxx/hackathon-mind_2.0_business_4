using System;
using System.Threading.Tasks;
using Hackaton.Data;
using Hackaton.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly ActivityPlatformDbContext _context;

    public AuthController(ActivityPlatformDbContext context)
    {
        _context = context;
    }

    public record RegisterUserRequest(string? Name, string? Role);
    public record UserResponse(int Id, string Name, string Role);

    [HttpPost("register")]
    public async Task<ActionResult<UserResponse>> Register([FromBody] RegisterUserRequest request)
    {
        if (request is null) return BadRequest();
        if (string.IsNullOrWhiteSpace(request.Name)) return BadRequest();

        var normalizedRole = NormalizeRole(request.Role);
        if (normalizedRole is null) return BadRequest();

        var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == normalizedRole);
        if (role is null)
        {
            role = new Role { RoleName = normalizedRole };
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
        }

        var user = new UserTable
        {
            FullName = request.Name,
            IdRole = role.IdRole
        };

        _context.UserTables.Add(user);
        await _context.SaveChangesAsync();

        return new UserResponse(user.IdUser, user.FullName!, role.RoleName!);
    }

    private static string? NormalizeRole(string? role)
    {
        if (string.IsNullOrWhiteSpace(role)) return null;

        var trimmed = role.Trim();
        if (string.Equals(trimmed, "Organizer", StringComparison.OrdinalIgnoreCase)) return "Organizer";
        if (string.Equals(trimmed, "Participant", StringComparison.OrdinalIgnoreCase)) return "Participant";

        return null;
    }
}