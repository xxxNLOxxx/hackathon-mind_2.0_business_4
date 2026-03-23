using HackathonApp.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// DB как заглушка пока
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseNpgsql("Host=localhost;Database=hackathon;Username=postgres;Password=1234"));

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();