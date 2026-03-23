using System.Collections.Generic;
using System.Threading.Tasks;
using Hackaton.Data;
using Hackaton.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("users")]
public class UserController : ControllerBase
{
    private readonly ActivityPlatformDbContext _context;

    public UserController(ActivityPlatformDbContext context)
    {
        _context = context;
    }

    public record UserProfileResponse(int Id, string Name, string Role);

    [HttpGet]
    public async Task<ActionResult<List<UserProfileResponse>>> GetAllParticipants()
    {
        var participantRoleId = await _context.Roles
            .Where(r => r.RoleName == "Participant")
            .Select(r => r.IdRole)
            .FirstOrDefaultAsync();

        if (participantRoleId == 0) return new List<UserProfileResponse>();

        var users = await _context.UserTables
            .Where(u => u.IdRole == participantRoleId)
            .OrderByDescending(u => u.TotalPoints)
            .Select(u => new UserProfileResponse(u.IdUser, u.FullName!, "Participant"))
            .ToListAsync();

        return users;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserProfileResponse>> GetParticipantById(int id)
    {
        var participantRoleId = await _context.Roles
            .Where(r => r.RoleName == "Participant")
            .Select(r => r.IdRole)
            .FirstOrDefaultAsync();

        if (participantRoleId == 0) return NotFound();

        var user = await _context.UserTables
            .Where(u => u.IdUser == id && u.IdRole == participantRoleId)
            .Select(u => new UserProfileResponse(u.IdUser, u.FullName!, "Participant"))
            .FirstOrDefaultAsync();

        if (user is null) return NotFound();

        return user;
    }
}