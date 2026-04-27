using System.ComponentModel.DataAnnotations;

namespace bebegim.Models
{
    public class Allergy
    {
        public int Id { get; set; }
        
        [Required]
        public int KidId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        public virtual Kid Kid { get; set; }
    }
}