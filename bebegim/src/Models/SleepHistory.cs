using System.ComponentModel.DataAnnotations;

namespace bebegim.Models
{
    public class SleepHistory
    {
        public int Id { get; set; }
        
        [Required]
        public int KidId { get; set; }

        [Required]
        public DateTime StartTime { get; set; } // Uykuya dalma saati

        [Required]
        public DateTime EndTime { get; set; } // Uyanma saati

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        public virtual Kid Kid { get; set; }
    }
}