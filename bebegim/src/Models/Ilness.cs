using System.ComponentModel.DataAnnotations;

namespace bebegim.Models
{
    public class Illness
    {
        public int Id { get; set; }
        
        [Required]
        public int KidId { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }

        public bool IsChronic { get; set; } = false; // Kronik mi?

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        public virtual Kid Kid { get; set; }
    }
}