using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HotDeskingApp.Data;
using HotDeskingApp.Models;

namespace HotDeskingApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController(AppDbContext context) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateReservation([FromBody] Reservation reservation)
    {
        var deskExists = await context.Desks.AnyAsync(d => d.Id == reservation.DeskId);
        if (!deskExists)
        {
            return BadRequest("Not Found.");
        }

        var isDeskOccupied = await context.Reservations.AnyAsync(r =>
            r.DeskId == reservation.DeskId &&
            r.ReservationDate.Date == reservation.ReservationDate.Date &&
            r.Status != ReservationStatus.Cancelled);

        if (isDeskOccupied)
        {
            return BadRequest("This desk is reserved for that day.");
        }

        reservation.Status = ReservationStatus.Confirmed;
        context.Reservations.Add(reservation);
        await context.SaveChangesAsync();

        return Ok(new { message = "The desk was successfully reserved.", reservationId = reservation.Id });
    }

    [HttpGet]
    public async Task<IActionResult> GetAllReservations()
    {
        var reservations = await context.Reservations
            .Include(r => r.Desk)
            .ToListAsync();

        return Ok(reservations);
    }
}