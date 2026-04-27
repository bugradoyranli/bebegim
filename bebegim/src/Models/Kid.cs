using System.ComponentModel.DataAnnotations;
namespace bebegim.Models;
public class Kid
{
    [Key]
    public int ID { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int ParentId { get; set; } = 0;

    [Required]
    public int Age { get; set; } = 0;

    [Required]
    public bool Gender { get; set; } = false;


    [Required]
    public double Weight { get; set; } = 0.0;


    public double Height { get; set; } = 0.0;
    public virtual User Parent { get; set; }
        public virtual ICollection<GrowHistory> GrowHistories { get; set; } = new List<GrowHistory>();
        public virtual ICollection<SleepHistory> SleepHistories { get; set; } = new List<SleepHistory>();
        public virtual ICollection<Vaccine> Vaccines { get; set; } = new List<Vaccine>();
        public virtual ICollection<FoodHistory> FoodHistories { get; set; } = new List<FoodHistory>();
        public virtual ICollection<Allergy> Allergies { get; set; } = new List<Allergy>();
        public virtual ICollection<Illness> Illnesses { get; set; } = new List<Illness>();

   
}