using System.ComponentModel.DataAnnotations;

namespace HotDeskingApp.Models
{
    public enum ReservationStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        public int UserId { get; set; }
        public User? User { get; set; } = null!;

        public int DeskId { get; set; }
        public Desk? Desk { get; set; } = null!;
    }
}