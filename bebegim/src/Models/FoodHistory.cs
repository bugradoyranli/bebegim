using System.ComponentModel.DataAnnotations;

namespace bebegim.Models
{
    public class FoodHistory
    {
        public int Id { get; set; }
        
        [Required]
        public int KidId { get; set; }

        [Required]
        public int FoodId { get; set; }

        [Required]
        public decimal Amount { get; set; } // 120 (ml), 15 (dk) veya 1 (porsiyon)

        [MaxLength(20)]
        public string? Unit { get; set; } // "ml", "dk", "porsiyon"

        [MaxLength(100)]
        public string? Detail { get; set; } // Emzirme için "Sol", "Sağ" veya ek notlar

        [Required]
        public DateTime Date { get; set; } // Ne zaman yedi?

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        public virtual Kid Kid { get; set; }
        public virtual Food Food { get; set; }
    }
}





