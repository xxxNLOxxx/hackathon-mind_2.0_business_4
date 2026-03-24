using Hackaton.Data;
using Hackaton.Models;
using Hackaton.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackaton.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ActivityPlatformDbContext _context;

        public EventsController(ActivityPlatformDbContext context)
        {
            _context = context;
        }

        // GET: api/events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents()
        {
            var events = await _context.EventTables
                .Include(e => e.IdOrganizerNavigation)
                .Include(e => e.EventRewards)
                    .ThenInclude(er => er.IdRewardNavigation)
                .Include(e => e.Participations)
                .Select(e => new EventDto
                {
                    Id = e.IdEvent,
                    Title = e.Title,
                    Description = e.Description,
                    Date = e.EventDate,
                    BasePoints = e.BasePoints ?? 0,
                    Difficulty = e.ComplexityCoeff ?? 1.0,
                    Category = e.IdCategoryNavigation != null ? e.IdCategoryNavigation.CategoryName : null,
                    OrganizerName = e.IdOrganizerNavigation.FullName,
                    Prizes = e.EventRewards
                        .Where(er => er.IdRewardNavigation != null)
                        .Select(er => er.IdRewardNavigation.RewardName)
                        .ToList(),
                    ParticipantsCount = e.Participations.Count(p => p.IdStatusParticipationNavigation != null &&
                                                                   p.IdStatusParticipationNavigation.Status == "Confirmed") // или по статусу
                })
                .ToListAsync();

            return Ok(events);
        }

        // POST: api/events
        [HttpPost]
        public async Task<ActionResult<EventTable>> CreateEvent(EventCreateDto dto)
        {
            // 1. Получение ID организатора
            if (!int.TryParse(Request.Headers["X-User-Id"], out int organizerId))
                return Unauthorized("Требуется идентификатор организатора");

            var organizer = await _context.UserTables.FindAsync(organizerId);
            if (organizer == null)
                return NotFound("Организатор не найден");

            // Проверка роли: нужно найти роль по IdRole или по названию
            // Предположим, что у организатора IdRole = 2 (например)
            if (organizer.IdRole != 2) // 2 - это роль организатора; нужно уточнить по вашей БД
                return Forbid("Только организатор может создавать мероприятия");

            // 2. Найти категорию по названию (если передана)
            int? categoryId = null;
            if (!string.IsNullOrEmpty(dto.Category))
            {
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == dto.Category);
                if (category != null)
                    categoryId = category.IdCategory;
            }

            // 3. Найти статус мероприятия "Активен" (предположим, IdStatus = 1)
            int activeStatusId = 1; // нужно определить по вашей БД

            // 4. Создание мероприятия
            var newEvent = new EventTable
            {
                Title = dto.Title,
                Description = dto.Description,
                EventDate = dto.Date,
                BasePoints = dto.BasePoints,
                ComplexityCoeff = dto.Difficulty,
                IdOrganizer = organizerId,
                IdCategory = categoryId,
                IdStatus = activeStatusId,
                CreatedAt = DateTime.UtcNow
            };

            _context.EventTables.Add(newEvent);
            await _context.SaveChangesAsync(); // получаем IdEvent

            // 5. Добавление призов (через Reward и EventReward)
            if (dto.PrizeNames != null && dto.PrizeNames.Any())
            {
                foreach (var prizeName in dto.PrizeNames)
                {
                    // Ищем существующую награду
                    var reward = await _context.Rewards.FirstOrDefaultAsync(r => r.RewardName == prizeName);
                    if (reward == null)
                    {
                        // Если не существует, создаём
                        reward = new Reward { RewardName = prizeName };
                        _context.Rewards.Add(reward);
                        await _context.SaveChangesAsync(); // сохраняем, чтобы получить IdReward
                    }

                    // Создаём связь EventReward
                    var eventReward = new EventReward
                    {
                        IdEvent = newEvent.IdEvent,
                        IdReward = reward.IdReward,
                        Title = prizeName, // или можно оставить пустым, зависит от модели
                        Quantity = 1       // или другое значение
                    };
                    _context.EventRewards.Add(eventReward);
                }
                await _context.SaveChangesAsync();
            }

            // 6. Увеличиваем счётчик мероприятий организатора (если нужно)
            // В UserTable нет поля для количества, но можно добавить, либо просто оставить без изменений
            // Если хотите, добавьте поле OrganizedEventsCount в UserTable и инкрементируйте
            // organizer.OrganizedEventsCount++; 

            return CreatedAtAction(nameof(GetEvents), new { id = newEvent.IdEvent }, newEvent);
        }
        [HttpGet("organizer/{organizerId}")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEventsByOrganizer(int organizerId)
        {
            var events = await _context.EventTables
                .Include(e => e.IdOrganizerNavigation)
                .Include(e => e.EventRewards)
                    .ThenInclude(er => er.IdRewardNavigation)
                .Include(e => e.Participations)
                .Where(e => e.IdOrganizer == organizerId)
                .Select(e => new EventDto
                {
                    Id = e.IdEvent,
                    Title = e.Title,
                    Description = e.Description,
                    Date = e.EventDate,
                    BasePoints = e.BasePoints ?? 0,
                    Difficulty = e.ComplexityCoeff ?? 1.0,
                    Category = e.IdCategoryNavigation != null ? e.IdCategoryNavigation.CategoryName : null,
                    OrganizerName = e.IdOrganizerNavigation.FullName,
                    Prizes = e.EventRewards
                        .Where(er => er.IdRewardNavigation != null)
                        .Select(er => er.IdRewardNavigation.RewardName)
                        .ToList(),
                    ParticipantsCount = e.Participations.Count(p => p.IdStatusParticipationNavigation != null &&
                                                                   p.IdStatusParticipationNavigation.Status == "Confirmed")
                })
                .ToListAsync();

            return Ok(events);
        }
    }
}