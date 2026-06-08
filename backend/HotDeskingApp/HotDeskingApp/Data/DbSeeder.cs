using HotDeskingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HotDeskingApp.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // 1. Upewniamy się, że baza danych fizycznie istnieje
        await context.Database.EnsureCreatedAsync();

        // 2. Jeśli w bazie są już jacyś użytkownicy, przerywamy działanie.
        // Zapobiega to dodawaniu tych samych testowych danych przy każdym włączeniu aplikacji.
        if (await context.Users.AnyAsync())
        {
            return;
        }

        // 3. Tworzymy domyślnego użytkownika pracownika
        var defaultUser = new User
        {
            FirstName = "Jan",
            LastName = "Kowalski",
            Email = "jan.kowalski@firma.pl"
        };
        context.Users.Add(defaultUser);

        // 4. Tworzymy domyślną lokalizację (biuro)
        var locationWarsaw = new Location
        {
            Name = "Biuro Warszawa - Piętro 1"
        };
        context.Locations.Add(locationWarsaw);

        // 5. Generujemy listę 5 testowych biurek przypisanych do powyższego biura
        var desks = new List<Desk>
        {
            new() { Number = "B-01", Location = locationWarsaw, IsAvailable = true },
            new() { Number = "B-02", Location = locationWarsaw, IsAvailable = true },
            new() { Number = "B-03", Location = locationWarsaw, IsAvailable = true },
            new() { Number = "B-04", Location = locationWarsaw, IsAvailable = true },
            new() { Number = "B-05", Location = locationWarsaw, IsAvailable = true }
        };
        context.Desks.AddRange(desks);

        // 6. Zapisujemy wszystko jednym kliknięciem w bazie danych SQL Server
        await context.SaveChangesAsync();
    }
}