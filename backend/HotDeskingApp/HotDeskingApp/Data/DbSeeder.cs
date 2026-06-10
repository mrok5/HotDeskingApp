using HotDeskingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace HotDeskingApp.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.Users.AnyAsync())
        {
            return;
        }

        var defaultUser = new User
        {
            FirstName = "Jan",
            LastName = "Kowalski",
            Email = "jan.kowalski@firma.pl",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123")
        };
        context.Users.Add(defaultUser);

        var locationWarsaw = new Location
        {
            Name = "Biuro Warszawa - Piętro 1"
        };
        context.Locations.Add(locationWarsaw);

        var desks = new List<Desk>
        {
            new() { Number = "B-01", Location = locationWarsaw, IsAvailable = true },
            new() { Number = "B-02", Location = locationWarsaw, IsAvailable = true },
            new() { Number = "B-03", Location = locationWarsaw, IsAvailable = true },
            new() { Number = "B-04", Location = locationWarsaw, IsAvailable = true },
            new() { Number = "B-05", Location = locationWarsaw, IsAvailable = true }
        };
        context.Desks.AddRange(desks);

        await context.SaveChangesAsync();
    }
}