using Hackaton.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ActivityPlatformDbContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Database=activity_platform_db;Username=postgres;Password=postgres"));

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();