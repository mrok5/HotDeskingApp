using HotDeskingApp.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// ---- SEEDOWANIE BAZY DANYCH (DODAJ TO TUTAJ) ----
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        // Wywołujemy nasz seeder asynchronicznie
        await DbSeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        // Jeśli coś pójdzie nie tak (np. brak połączenia z bazą), logujemy błąd w konsoli
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Wystąpił błąd podczas automatycznego seedowania bazy danych.");
    }
}

app.Run();
