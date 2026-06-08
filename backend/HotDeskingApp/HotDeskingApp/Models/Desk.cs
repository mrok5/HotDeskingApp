namespace HotDeskingApp.Models
{
    public class Desk
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public bool IsAvailable { get; set; } = true;

        public int LocationId { get; set; }
        public Location? Location { get; set; } = null!;

        public ICollection<Reservation> Reservations{ get; set; } = [];
    }
}