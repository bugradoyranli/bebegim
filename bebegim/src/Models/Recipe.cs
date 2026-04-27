using System.ComponentModel.DataAnnotations;

namespace bebegim.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Ingredients { get; set; } // Malzemeler

        [Required]
        public string Preparation { get; set; } // Hazırlanışı

        public int MinAgeMonths { get; set; } // Minimum kaç aylık bebekler için uygun? (Örn: 6)

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
    }
}