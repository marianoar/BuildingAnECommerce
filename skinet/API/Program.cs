var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// de aqui para arriba se considera Service
var app = builder.Build();
// de aqui para abajo se considera Middleware

app.MapControllers();

app.Run();
