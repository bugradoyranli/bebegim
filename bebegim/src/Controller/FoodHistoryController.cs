using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bebegim.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bebegim.Data;

namespace bebegim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodHistoryController : ControllerBase
    {
        private readonly BebegimDbContext _context;

        public FoodHistoryController(BebegimDbContext context)
        {
            _context = context;
        }

        // 1. Standart Beslenme Kaydı (Var olan bir yiyeceği seçince)
        [HttpPost("Record")]
        public async Task<IActionResult> CreateFeedingRecord([FromBody] FeedingRecordDto recordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
                var history = new FoodHistory
                {
                    KidId = recordDto.KidId,
                    FoodId = recordDto.FoodId,
                    Amount = recordDto.Amount,
                    Unit = recordDto.Unit,
                    Detail = recordDto.Detail,
                    Date = recordDto.Date ?? DateTime.UtcNow,
                    AddedDate = DateTime.UtcNow
                };

                _context.FoodHistories.Add(history);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Kayıt başarıyla eklendi.", id = history.Id });
            

        }

        // 2. Yeni Özel Yemek Oluşturma ve Kaydetme
        // Kullanıcı yeni bir yemek adı ve açıklama girince burası çalışır
        [HttpPost("RecordCustom")]
        public async Task<IActionResult> CreateCustomFeedingRecord([FromBody] CustomFeedingRecordDto customDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            
                // Önce yiyecek daha önce bu çocuk için eklenmiş mi kontrol et
                var food = await _context.Foods.Where(f => f.KidId == customDto.KidId && f.Name.ToLower() == customDto.FoodName.ToLower())
                .FirstOrDefaultAsync(); 
              
                  

                // Eğer yiyecek yoksa yeni oluştur
                if (food == null)
                {
                    food = new Food
                    {   KidId = customDto.KidId,
                        Name = customDto.FoodName,
                        Description = customDto.Description,
                        AddedDate = DateTime.UtcNow
                    };
                    _context.Foods.Add(food);
                    await _context.SaveChangesAsync();
                }

                // Şimdi geçmişe kaydet
                var history = new FoodHistory
                {
                    KidId = customDto.KidId,
                    FoodId = food.Id,
                    Amount = customDto.Amount,
                    Unit = customDto.Unit,
                    Detail = customDto.Detail,
                    Date = customDto.Date ?? DateTime.UtcNow,
                    AddedDate = DateTime.UtcNow
                };

                _context.FoodHistories.Add(history);
                await _context.SaveChangesAsync();

              

                return Ok(new { message = "Yeni yemek tanımlandı ve kaydedildi.", foodId = food.Id, historyId = history.Id });

        }


               // 4. Günlük Geçmiş (Bugün)
        [HttpGet("DailyHistory/{kidId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetDailyHistory(int kidId)
        {
            var today = DateTime.UtcNow.Date;
            return await FetchHistoryByDate(kidId, today);
        }

        // 5. Belirli Bir Güne Göre Geçmişi Getirme
        // Örn: api/Food/History/5/2023-10-25
        [HttpGet("GetHistoryByDate/{kidId}/{date}")]
        public async Task<ActionResult<IEnumerable<object>>> GetHistoryByDate(int kidId, DateTime date)
        {
            return await FetchHistoryByDate(kidId, date.Date);
        }

        // Tekrarı önlemek için ortak veri çekme metodu
        private async Task<ActionResult<IEnumerable<object>>> FetchHistoryByDate(int kidId, DateTime targetDate)
        {
            var history = await _context.FoodHistories
                .Include(fh => fh.Food)
                .Where(fh => fh.KidId == kidId && fh.Date.Date == targetDate)
                .OrderByDescending(fh => fh.Date)
                .Select(fh => new
                {
                    fh.Id,
                    FoodName = fh.Food.Name,
                    fh.Amount,
                    fh.Unit,
                    fh.Detail,
                    Time = fh.Date.ToString("HH:mm")
                })
                .ToListAsync();

                if (history.Count == 0)
                {
                    return Ok(new { message = "Bu tarihte herhangi bir beslenme kaydı bulunmamaktadır.", data = history });
                }

            return Ok(history);
        }

        [HttpDelete("Record/{id}")]
        public async Task<IActionResult> DeleteRecord(int id)
        {
            var record = await _context.FoodHistories.FindAsync(id);
            if (record == null) return NotFound();

            _context.FoodHistories.Remove(record);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Kayıt silindi." });
        }
    }

   

    
}