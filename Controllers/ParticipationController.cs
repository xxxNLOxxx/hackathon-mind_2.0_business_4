using Hackaton.Data;
using Hackaton.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Hackaton.Controllers
{
    [ApiController]
    [Route("participation")]

    public class ParticipationController : ControllerBase
    {
        private readonly ActivityPlatformDbContext _context;

        public ParticipationController(ActivityPlatformDbContext context)
        {
            _context = context;
        }

        // 1. POST: api/participation/join
        [HttpPost("join")]
        public async Task<IActionResult> Join([FromBody] ParticipationRequest request)
        {
            // Проверка на дубликат (используем имена из схемы: id_user, id_event)
            var exists = await _context.Participations
                .AnyAsync(p => p.IdUser == request.IdUser && p.IdEvent == request.IdEvent);

            if (exists)
                return BadRequest("Вы уже зарегистрированы на это мероприятие.");

            var newParticipation = new Participation
            {
                IdUser = request.IdUser,
                IdEvent = request.IdEvent,
                IdStatusParticipation = 1, // 1 = 'Registered' (согласно таблице status_participation)
                QrCodeHash = Guid.NewGuid().ToString(), // Генерация уникального хеша
                PointsEarned = 0,
            };

            _context.Participations.Add(newParticipation);
            await _context.SaveChangesAsync();

            return Ok(newParticipation);
        }

        // 2. POST: api/participation/confirm/{id}
        [HttpPost("confirm/{id}")]
        public async Task<IActionResult> Confirm(int id)
        {
            // Загружаем участие вместе с данными о событии и пользователе
            var participation = await _context.Participations
                .Include(p => p.IdEventNavigation) // Навигационное свойство к Event
                .Include(p => p.IdUserNavigation)  // Навигационное свойство к User
                .FirstOrDefaultAsync(p => p.IdParticipation == id);

            if (participation == null)
                return NotFound("Запись об участии не найдена.");

            if (participation.IdStatusParticipation == 2) // 2 = 'Confirmed'
                return BadRequest("Участие уже было подтверждено ранее.");
            double calculatedPoints = (participation.IdEventNavigation.BasePoints ?? 0) * (participation.IdEventNavigation.ComplexityCoeff ?? 1);

            // Обновляем статус участия
            participation.IdStatusParticipation = 2; // Статус 'Confirmed'
            participation.PointsEarned = calculatedPoints;
            participation.ConfirmedAt = DateTime.UtcNow;

            // Обновляем общий рейтинг пользователя (total_points в схеме - Integer)
            participation.IdUserNavigation.TotalPoints += (int)calculatedPoints;

            // (Опционально) Добавляем запись в points_history, если она есть в контексте
            var history = new PointsHistory
            {
                IdUser = participation.IdUser,
                PointsChange = calculatedPoints,
                Reason = $"Участие в: {participation.IdEventNavigation.Title}",
                CreatedAt = DateTime.UtcNow
            };
            _context.PointsHistories.Add(history);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Участие подтверждено",
                points_added = calculatedPoints,
                new_total_rating = participation.IdUserNavigation.TotalPoints
            });
        }

        // 3. GET: api/participation/user/{id}
        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserHistory(int id)
        {
            var history = await _context.Participations
                .Where(p => p.IdUser == id)
                .Include(p => p.IdEventNavigation)
                .Select(p => new {
                    p.IdParticipation,
                    EventTitle = p.IdEventNavigation.Title,
                    p.IdEventNavigation.EventDate,
                    p.PointsEarned,
                    StatusId = p.IdStatusParticipation,
                    p.ConfirmedAt
                })
                .OrderByDescending(p => p.EventDate)
                .ToListAsync();

            return Ok(history);
        }
    }

    public class ParticipationRequest
    {
        public int IdUser { get; set; }
        public int IdEvent { get; set; }
    }
}
       
