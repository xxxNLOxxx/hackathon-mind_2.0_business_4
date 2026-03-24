using Hackaton.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using Hackaton.Models;
using Microsoft.EntityFrameworkCore;
namespace Hackaton.Controllers 
{ 

    [ApiController]
    [Route("api/[controller]")]
    public class RatingController : ControllerBase
    {
        private readonly ActivityPlatformDbContext _context;

        public RatingController(ActivityPlatformDbContext context)
        {
            _context = context;
        }

        // 1. GET /rating/top — Возвращает топ-100 пользователей по TotalScore
        [HttpGet("top")]
        public async Task<ActionResult<IEnumerable<UserRating>>> GetTop100()
        {
            var topUsers = await _context.UserTables
                .OrderByDescending(u => u.TotalPoints)
                .Take(100)
                .Select((u, index) => new UserRating
                {
                    UserId = u.IdUser,
                    FullName = u.FullName, 
                    TotalScore = u.TotalPoints,
                    Rank = 0 
                })
                .ToListAsync();

            // Проставляем ранг (1, 2, 3...)
            for (int i = 0; i < topUsers.Count; i++)
            {
                topUsers[i].Rank = i + 1;
            }

            return Ok(topUsers);
        }

        // 2. GET /rating/user/{id} — Возвращает рейтинг конкретного пользователя
        [HttpGet("user/{id}")]
        public async Task<ActionResult<UserRating>> GetUserRating(int id)
        {
            var user = await _context.UserTables.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { Message = "Пользователь не найден" });
            }


            int rank = await _context.UserTables
                .CountAsync(u => u.TotalPoints > user.TotalPoints) + 1;

            var result = new UserRating
            {
                UserId = user.IdUser,
                FullName = user.FullName,
                TotalScore = user.TotalPoints,
                Rank = rank
            };

            return Ok(result);
        }
    }
}

