using HotDeskingApp.Controllers;
using HotDeskingApp.Data;
using HotDeskingApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotDeskingApp.Tests;

public class ReservationsControllerTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new AppDbContext(options);

        context.Desks.Add(new Desk { Id = 1, Number = "B-01" });
        context.Users.Add(new User { Id = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jan@firma.pl" });
        context.SaveChanges();

        return context;
    }

    [Fact]
    public async Task CreateReservation_ShouldReturnOk_WhenDeskIsFree()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        var controller = new ReservationsController(context);

        var newReservation = new Reservation
        {
            DeskId = 1,
            UserId = 1,
            ReservationDate = new DateTime(2026, 06, 15)
        };

        // Act 
        var result = await controller.CreateReservation(newReservation);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }

    [Fact]
    public async Task CreateReservation_ShouldReturnBadRequest_WhenDeskIsAlreadyBookedOnThatDay()
    {
        // Arrange
        var context = GetInMemoryDbContext();
        context.Reservations.Add(new Reservation
        {
            Id = 10,
            DeskId = 1,
            UserId = 1,
            ReservationDate = new DateTime(2026, 06, 15),
            Status = ReservationStatus.Confirmed
        });
        await context.SaveChangesAsync();

        var controller = new ReservationsController(context);

        var duplicateReservation = new Reservation
        {
            DeskId = 1,
            UserId = 1,
            ReservationDate = new DateTime(2026, 06, 15)
        };

        // Act
        var result = await controller.CreateReservation(duplicateReservation);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("This desk is reserved for that day.", badRequestResult.Value);
    }
}