using System.ComponentModel.DataAnnotations;

namespace bebegim.Models
{
    public class Vaccine
    {
        public int Id { get; set; }
        
        [Required]
        public int KidId { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required]
        public DateTime PlannedDate { get; set; } // Planlanan aşı tarihi

        public DateTime? HappenedDate { get; set; } // Opsiyonel: Yapıldıysa bu tarih dolar

        public bool IsReminderSent { get; set; } = false; // Bildirim atıldı mı?

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        public virtual Kid Kid { get; set; }
    }
}