using Hackaton.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ActivityPlatformDbContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Database=hackathon;Username=postgres;Password=1337"));
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();