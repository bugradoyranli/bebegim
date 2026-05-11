using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bebegim.Data;
using bebegim.Models;

namespace bebegim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineController : ControllerBase
    {
        private readonly BebegimDbContext _dbContext;

        public VaccineController(BebegimDbContext dbContext)
        {  
            _dbContext = dbContext;
        }

        // ==========================================
        // 1. CREATE (Yeni Aşı Planı Ekleme)
        // URL: POST api/vaccine/{kidId}
        // ==========================================
        [HttpPost("{kidId}")]
        public async Task<IActionResult> Add(int kidId, [FromBody] VaccineCreateDto dto)
        {
            await EnsureKidExistsAsync(kidId);

            var newVaccine = new Vaccine
            {
                KidId = kidId,
                Name = dto.Name,
                Description = dto.Description,
                PlannedDate = dto.PlannedDate,
                HappenedDate = dto.HappenedDate, 
                AddedDate = DateTime.UtcNow,
                IsReminderSent = false
            };
            
            _dbContext.Vaccines.Add(newVaccine);
            await _dbContext.SaveChangesAsync();

            return Ok(new ApiResponse<VaccineResponseDto> 
            { 
                Message = "Aşı planı başarıyla eklendi", 
                Data = MapToDto(newVaccine) 
            });
        }

        // ==========================================
        // 2. READ (Bebeğin Tüm Aşılarını Getirme)
        // URL: GET api/vaccine/kid/{kidId}
        // ==========================================
        [HttpGet("kid/{kidId}")]
        public async Task<IActionResult> GetByKidId(int kidId)
        {
            await EnsureKidExistsAsync(kidId);

            var vaccines = await _dbContext.Vaccines
                                    .Where(v => v.KidId == kidId)
                                    // Tarihi yaklaşan aşılar en üstte görünsün diye planlanan tarihe göre sıraladık
                                    .OrderBy(v => v.PlannedDate) 
                                    .ToListAsync();

            var vaccineDtos = vaccines.Select(v => MapToDto(v)).ToList();

            return Ok(new ApiResponse<List<VaccineResponseDto>>
            {
                Message = vaccineDtos.Count > 0 ? "Aşı takvimi başarıyla getirildi" : "Henüz bir aşı planı bulunmuyor.",
                Data = vaccineDtos
            });
        }


#region "VaccineUpdate"

 [HttpPut("{vaccineId}")]
        public async Task<IActionResult> Update(int vaccineId, [FromBody] VaccineUpdateDto dto)
        {
            var vaccine = await GetVaccineByIdAsync(vaccineId);

            vaccine.Name = dto.Name;
            vaccine.Description = dto.Description;
            vaccine.PlannedDate = dto.PlannedDate;
            vaccine.HappenedDate = dto.HappenedDate;

            _dbContext.Vaccines.Update(vaccine);
            await _dbContext.SaveChangesAsync();

            return Ok(new ApiResponse<VaccineResponseDto>
            {
                Message = "Aşı bilgileri güncellendi",
                Data = MapToDto(vaccine)
            });
        }


#endregion




 /// <summary>
/// belirli bir aşıyı sadece "yapıldı" olarak işaretlemek için kullanılır.
        /// </summary>
        /// <remarks>
/// Bu işlemde aşının yapıldığı tarih otomatik olarak anlık zaman atanır (veya opsiyonel olarak gönderilen tarih kullanılır).
/// UI tarafında bu metod, aşı planının yanında bulunan "Tik" (Check) butonuna basıldığında çağrılmalıdır. 
/// Eğer aşı zaten yapıldı olarak işaretlenmişse, kullanıcıya hata döndürülür. 
/// </remarks>
        [HttpPatch("{vaccineId}/mark-done")]
        public async Task<IActionResult> MarkAsDone(int vaccineId, DateTime? happenedDate = null)
        {
            // Bu metod UI tarafında bir "Tik" (Check) butonuna basıldığında çağrılır.
            var vaccine = await GetVaccineByIdAsync(vaccineId);

            if (vaccine.HappenedDate != null)
                return BadRequest(new ApiResponse<object> { Message = "Bu aşı zaten yapıldı olarak işaretlenmiş!" ,Success = false });

            // Aşının yapıldığı tarihi o anki zaman olarak atıyoruz
            vaccine.HappenedDate = happenedDate ?? DateTime.UtcNow;

            _dbContext.Vaccines.Update(vaccine);
            await _dbContext.SaveChangesAsync();

            return Ok(new ApiResponse<VaccineResponseDto>
            {
                Message = "Aşı başarıyla yapıldı olarak işaretlendi",
                Data = MapToDto(vaccine)
            });
        }
        /// <summary>
        /// Bebek için yapılmış tüm aşıları getirir. Eğer henüz yapılmış bir aşı yoksa boş liste döner.
        /// </summary>
        /// <param name="kidId"></param>
        /// <returns></returns>
        [HttpGet("done/{kidId}")]
        public async Task<IActionResult> GetDoneVaccines(int kidId)
        {
            await EnsureKidExistsAsync(kidId);

            var doneVaccines = await _dbContext.Vaccines
                                        .Where(v => v.KidId == kidId && v.HappenedDate != null)
                                        .OrderByDescending(v => v.HappenedDate) // En son yapılan aşılar en üstte
                                        .ToListAsync();

            var doneVaccineDtos = doneVaccines.Select(v => MapToDto(v)).ToList();
            return Ok(new ApiResponse<List<VaccineResponseDto>>
            {
                Message = doneVaccineDtos.Count > 0 ? "Yapılmış aşılar başarıyla getirildi" : "Henüz yapılmış bir aşı kaydı bulunmuyor.",
                Data = doneVaccineDtos
            });
        }


        [HttpDelete("{vaccineId}")]
        public async Task<IActionResult> Delete(int vaccineId)
        {
            var vaccine = await GetVaccineByIdAsync(vaccineId);

            _dbContext.Vaccines.Remove(vaccine);
            await _dbContext.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Message = "Aşı kaydı başarıyla silindi",
                Data = null
            });
        }

      
        private async Task EnsureKidExistsAsync(int kidId)
        {
            var kidExists = await _dbContext.Kids.AnyAsync(k => k.ID == kidId);
            if (!kidExists) 
                throw new Exception("Belirtilen ID'ye sahip bebek bulunamadı!");
        }

        private async Task<Vaccine> GetVaccineByIdAsync(int vaccineId)
        {
            var vaccine = await _dbContext.Vaccines.FindAsync(vaccineId);
            if (vaccine == null)
                throw new Exception("Aşı kaydı bulunamadı!");
            
            return vaccine;
        }

        private VaccineResponseDto MapToDto(Vaccine vaccine)
        {
            return new VaccineResponseDto
            {
                Id = vaccine.Id,
                KidId = vaccine.KidId,
                Name = vaccine.Name,
                Description = vaccine.Description,
                PlannedDate = vaccine.PlannedDate,
                HappenedDate = vaccine.HappenedDate,
                IsReminderSent = vaccine.IsReminderSent,
                AddedDate = vaccine.AddedDate
            };
        }
    }
}