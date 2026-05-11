using System.ComponentModel.DataAnnotations;
namespace bebegim.Models;

public class UserLoginDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class UserRegisterDto
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Surname { get; set; }
    [Required]
    [EmailAddress(ErrorMessage = "Geçersiz e-posta formatı.")] 
    public string Email { get; set; }
    [Required]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Şifre en az 8 karakter olmalıdır.")]
    public string PasswordHash { get; set; }
}





public class KidResponseDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int Age { get; set; }
        public bool Gender { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
    }



    public class KidUpdateDto
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
    }




    public class SleepHistoryCreateDto
    {
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; } 
    }

    // Güncelleme yaparken uyanma saatini eklemek/değiştirmek isteyebiliriz
    public class SleepHistoryUpdateDto
    {
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

    // SwiftUI tarafına (Response olarak) gidecek temizlenmiş model
    public class SleepHistoryResponseDto
    {
        public int Id { get; set; }
        public int KidId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime AddedDate { get; set; }
    }






    // Aşı ilk planlanırken girilecek veriler
    public class VaccineCreateDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        
        public DateTime? HappenedDate { get; set; } // Eğer aşı planlanırken yapıldı olarak işaretlenirse bu alan dolabilir 
        public DateTime? PlannedDate { get; set; }

    }

    // Aşının genel bilgilerini güncellemek için
    public class VaccineUpdateDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime PlannedDate { get; set; }
        public DateTime? HappenedDate { get; set; }
    }

    // SwiftUI'a dönecek temiz liste (Döngüsüz)
    public class VaccineResponseDto
    {
        public int Id { get; set; }
        public int KidId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? PlannedDate { get; set; }
        public DateTime? HappenedDate { get; set; }
        public bool IsReminderSent { get; set; }
        public DateTime AddedDate { get; set; }
    }
