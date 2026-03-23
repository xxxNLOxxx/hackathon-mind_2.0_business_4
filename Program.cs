using Hackaton.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// DB как заглушка пока
// builder.Services.AddDbContext<ActivityPlatformDbContext>(options =>
//     options.UseNpgsql("Host=localhost;Database=hackathon;Username=postgres;Password=1234"));
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();