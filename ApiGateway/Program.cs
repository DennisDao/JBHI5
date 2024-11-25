using ApiGateway;
using ApiGateway.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<RateLimitService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseMiddleware<RateLimitMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
