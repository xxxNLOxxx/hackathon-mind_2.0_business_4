using Microsoft.EntityFrameworkCore;

namespace HackathonApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // ПОКА ПУСТО, это заглушка которую потом заменят сгенерированные модели из постгреса
}