using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bebegim.Data;
using bebegim.Models;
namespace bebegim.Controller;
[ApiController]
[Route("api/[controller]")]
public class KidController : ControllerBase
{
    private readonly BebegimDbContext dbContext;
    private readonly ILogger<KidController> logger;

    public KidController(BebegimDbContext _context, ILogger<KidController> _logger)
    {  
        dbContext = _context;
        logger = _logger;
    }
   // userid'yi URL'den almak daha sağlıklı bir REST mimarisidir. (Örn: api/kid/5)
[HttpPost("{userid}")] 
public async Task<IActionResult> Create(int userid, [FromBody] KidCreateDto dto)
{
    if (dto == null)
        throw new Exception("Bebek verisi boş olamaz!"); // Middleware yakalayacak

    var isValidUser = await dbContext.Users.FindAsync(userid);
    if (isValidUser == null)
        throw new Exception("Bağlı kullanıcı bulunamadı!"); // Middleware yakalayacak

    // 1. Yeni Bebeği Oluşturuyoruz
    var newKid = new Kid
    {
        Name = dto.Name,
        Age = dto.Age,
        Gender = dto.Gender,
        ParentId = userid,
        
        // 2. Navigation Property sayesinde, bebeği eklerken 
        // İLK boy ve kilo kaydını da GrowHistory tablosuna tek seferde atıyoruz!
        GrowHistories = new List<GrowHistory>
        {
            new GrowHistory 
            { 
                Weight = dto.Weight, 
                Height = dto.Height 
            }
        }
    };

    dbContext.Kids.Add(newKid);
    await dbContext.SaveChangesAsync();


    var response = new ApiResponse<Kid>
    {
        Message = "Kid created successfully",
        Data = newKid
    };
    // Veritabanında oluşan yeni nesneyi (ID'si ile birlikte) SwiftUI'a dönüyoruz
    return Ok(response);
}
    

    [HttpGet]
     public async Task<IActionResult> Get()
     {
         var kids = await dbContext.Kids.ToListAsync();
        if (kids == null || kids.Count == 0)
        {
            throw new Exception("Çocuk verisi bulunamadı!"); // Middleware yakalayacak
        }
     
         var response = new ApiResponse<List<Kid>>
    {
        
        Message = "Çocuk başarıyla oluşturuldu",
        Data = kids
    };
    return Ok(response);
     }
    // Buraya kid ile ilgili endpointler gelecek

    
}