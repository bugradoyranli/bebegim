using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bebegim.Data;
using bebegim.Models;

namespace bebegim.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // Bu attribute otomatik validasyonlar için çok önemlidir
    public class SleepHistoryController : ControllerBase
    {
        private readonly BebegimDbContext _dbContext;
        private readonly ILogger<SleepHistoryController> _logger;

        public SleepHistoryController(BebegimDbContext dbContext, ILogger<SleepHistoryController> logger)
        {  
            _dbContext = dbContext;
            _logger = logger;
        }


 /// <summary>
 /// 
 /// Yeni bir uyku kaydı ekler.
/// Bebek uyandığında, uyku kaydının EndTime'ını güncellemek için bu endpoint çağrılmalıdır.
        /// </summary>
        /// <remarks>
/// Bebek "Uyudu" butonuna basıldığında bu endpoint çağrılmalıdır. 
/// EndTime (Uyanma saati) gönderilmesine gerek yoktur.
/// dönen id tutulmalıdır çünkü uyandığında bu kayda EndTime eklemek için kullanılacaktır.
/// </remarks>
        [HttpPost("{kidId}")]
        public async Task<IActionResult> Add(int kidId, [FromBody] SleepHistoryCreateDto dto)
        {
            await EnsureKidExistsAsync(kidId);

            var newSleep = new SleepHistory
            {
                KidId = kidId,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                AddedDate = DateTime.UtcNow 
            };
            
            _dbContext.SleepHistories.Add(newSleep);
            await _dbContext.SaveChangesAsync();

            return Ok(new ApiResponse<SleepHistoryResponseDto> 
            { 
                Message = "Uyku geçmişi başarıyla eklendi", 
                Data = MapToDto(newSleep) 
            });
        }

        // (Bebeğin Tüm Uyku Kayıtlarını Getirme)
        // URL: GET api/sleephistory/kid/{kidId}
        [HttpGet("kid/{kidId}")]
        public async Task<IActionResult> GetByKidId(int kidId)
        {
            await EnsureKidExistsAsync(kidId);

            var sleepHistories = await _dbContext.SleepHistories
                                        .Where(sh => sh.KidId == kidId)
                                        .OrderByDescending(sh => sh.StartTime)
                                        .ToListAsync();

            var sleepDtos = sleepHistories.Select(sh => MapToDto(sh)).ToList();

            return Ok(new ApiResponse<List<SleepHistoryResponseDto>>
            {
                Message = sleepDtos.Count > 0 ? "Uyku geçmişi başarıyla getirildi" : "Henüz bir uyku kaydı bulunmuyor.",
                Data = sleepDtos
            });
        }

    
// 3. UPDATE (Uyku Kaydını Güncelleme - Örn: Bebek uyandı)
// URL: PUT api/sleephistory/{sleepId}
        

 /// <summary>
/// Bebek uyandığında, uyku kaydının EndTime'ını güncellemek için bu endpoint çağrılmalıdır.
/// </summary>
 /// <remarks>
/// Bebek uyandığında, uyku kaydının EndTime'ını güncellemek için bu endpoint çağrılmalıdır.
/// Gönderilen sleepId, güncellenecek uyku kaydının ID'si olmalıdır.
/// StartTime ve EndTime gönderilmesi zorunludur. EndTime, StartTime'dan sonra olmalıdır.
/// </remarks>
        [HttpPut("{sleepId}")]
        public async Task<IActionResult> Update(int sleepId, [FromBody] SleepHistoryUpdateDto dto)
        {
            var sleepRecord = await _dbContext.SleepHistories.FindAsync(sleepId);
            if (sleepRecord == null)
                throw new Exception("Uyku kaydı bulunamadı!");

            if (dto.EndTime <= dto.StartTime)
                throw new Exception("EndTime, StartTime'dan sonra olmalıdır!");

            sleepRecord.StartTime = dto.StartTime;
            sleepRecord.EndTime = dto.EndTime; // Bebek uyandığında bu alanı doldurabiliriz

            _dbContext.SleepHistories.Update(sleepRecord);
            await _dbContext.SaveChangesAsync();

            return Ok(new ApiResponse<SleepHistoryResponseDto>
            {
                Message = "Uyku kaydı güncellendi",
                Data = MapToDto(sleepRecord)
            });
        }

        // ==========================================
        // 4. DELETE (Uyku Kaydını Silme)
        // URL: DELETE api/sleephistory/{sleepId}
        // ==========================================
        [HttpDelete("{sleepId}")]
        public async Task<IActionResult> Delete(int sleepId)
        {
            var sleepRecord = await _dbContext.SleepHistories.FindAsync(sleepId);
            if (sleepRecord == null)
                throw new Exception("Uyku kaydı bulunamadı!");

            _dbContext.SleepHistories.Remove(sleepRecord);
            await _dbContext.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Message = "Uyku kaydı başarıyla silindi",
                Data = null
            });
        }

        // =========================================================================
        // YARDIMCI METODLAR (HELPER METHODS)
        // =========================================================================

        private async Task EnsureKidExistsAsync(int kidId)
        {
            var kidExists = await _dbContext.Kids.AnyAsync(k => k.ID == kidId);
            if (!kidExists) 
                throw new Exception("Belirtilen ID'ye sahip bebek bulunamadı!");
        }

        private SleepHistoryResponseDto MapToDto(SleepHistory sleepHistory)
        {
            return new SleepHistoryResponseDto
            {
                Id = sleepHistory.Id,
                KidId = sleepHistory.KidId,
                StartTime = sleepHistory.StartTime,
                EndTime = sleepHistory.EndTime,
                AddedDate = sleepHistory.AddedDate
            };
        }
    }
}