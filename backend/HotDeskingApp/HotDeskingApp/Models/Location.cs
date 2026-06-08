using System.ComponentModel.DataAnnotations;

namespace HotDeskingApp.Models
{
    public class Location
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public ICollection<Desk> Desks { get; set; } = new List<Desk>();
    }
}
