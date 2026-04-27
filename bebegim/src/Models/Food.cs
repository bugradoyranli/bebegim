using System.ComponentModel.DataAnnotations;

namespace bebegim.Models
{
    public class Food
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public int? ParentId { get; set; } // Kategori mantığı için (Örn: Meyveler -> Elma)

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
    }

}