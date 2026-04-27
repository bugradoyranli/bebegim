using System.ComponentModel.DataAnnotations;

namespace bebegim.Models
{
    public class GrowHistory
    {
        public int Id { get; set; }
        
        [Required]
        public int KidId { get; set; }

        [Required]
        public double Weight { get; set; } = 0; // Kilogram cinsinden (Örn: 5.4)

        [Required]
        public double Height { get; set; } = 0; // Santimetre cinsinden (Örn: 60.5)

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        public virtual Kid Kid { get; set; }
    }
}
