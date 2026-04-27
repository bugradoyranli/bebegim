using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bebegim.Data;
using bebegim.Models;
public class SleepHistoryController : ControllerBase
{
    

    private readonly ILogger<SleepHistory> logger;
    private readonly BebegimDbContext dbContext;


    public SleepHistoryController(BebegimDbContext _context, ILogger<SleepHistory> _logger)
    {  
        dbContext = _context;
        logger = _logger;

    }

    [HttpPost("add-sleep-history/{kidId}")]
public async Task<IActionResult> Add(int kidId, [FromBody] SleepHistory sleepHistory)
{
    // 1. Önce böyle bir bebek var mı kontrol etmek güvenlidir
    var kidExists = await dbContext.Kids.AnyAsync(k => k.ID == kidId);
    if (!kidExists) throw new Exception("Bebek bulunamadı!");

    sleepHistory.KidId = kidId;
    
    dbContext.SleepHistories.Add(sleepHistory);
    await dbContext.SaveChangesAsync();

    // SaveChangesAsync sonrası 'sleepHistory' objesi veritabanındaki ID'sini otomatik alır.
    // Tekrar FindAsync yapmana gerek yok. Direkt elimizdeki nesneyi dönüyoruz.
    return Ok(new ApiResponse<SleepHistory> 
    { 
        Message = "Uyku geçmişi başarıyla eklendi", 
        Data = sleepHistory 
    });
}

[HttpGet("get-sleep-history/{kidId}")]
public async Task<IActionResult> Get(int kidId)
{
    // Veritabanından bebeğe ait tüm kayıtları çekiyoruz
    var sleepHistories = await dbContext.SleepHistories
                                .Where(sh => sh.KidId == kidId)
                                .OrderByDescending(sh => sh.StartTime) // En yeni uyku en üstte gelsin
                                .ToListAsync();

    // Liste boşsa hata fırlatmak yerine, boş listeyi düzgün bir mesajla dönüyoruz
    return Ok(new ApiResponse<List<SleepHistory>>
    {
        Message = sleepHistories.Count > 0 ? "Uyku geçmişi başarıyla getirildi" : "Henüz bir uyku kaydı bulunmuyor.",
        Data = sleepHistories
    });
}
}