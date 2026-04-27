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
        public DateTime Date { get; set; } // Ne zaman yedi?

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

        public virtual Kid Kid { get; set; }
        public virtual Food Food { get; set; }
    }
}