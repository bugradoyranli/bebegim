using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace bebegim.Models
{
    public class Food
    {   
        [Key]
        public int Id { get; set; }

        public int KidId { get; set; } 

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime AddedDate { get; set; } = DateTime.UtcNow;
    }

}