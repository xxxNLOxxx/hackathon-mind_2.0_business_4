using System;
using System.Threading.Tasks;
using Hackaton.Data;
using Hackaton.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ActivityPlatformDbContext _context;

    public AuthController(ActivityPlatformDbContext context)
    {
        _context = context;
    }

    public record RegisterUserRequest(string? Name, string? Role);
    public record LoginRequest(string Email, string Password);
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

    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> Login([FromBody] LoginRequest request)
    {
        // Ищем пользователя по email (используем email как логин)
        var user = await _context.UserTables
            .Include(u => u.IdRoleNavigation)
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
        {
            return Unauthorized(new { message = "Пользователь не найден" });
        }

        // Временно: сравниваем пароль как есть (потом добавить хэширование)
        if (user.Pwd != request.Password)
        {
            return Unauthorized(new { message = "Неверный пароль" });
        }

        return new UserResponse(user.IdUser, user.FullName!, user.IdRoleNavigation?.RoleName ?? "Participant");
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