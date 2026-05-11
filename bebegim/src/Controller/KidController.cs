using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bebegim.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using bebegim.Data;

namespace bebegim.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KidController : ControllerBase
    {
        private readonly BebegimDbContext _dbContext;

        public KidController(BebegimDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ==========================================
        // 1. CREATE (Yeni Bebek Ekleme)
        // ==========================================



        /// <summary>
/// Yeni bir bebek oluşturur.
/// </summary>
/// <remarks>
/// Yeni bir bebek oluşturmak için kullanılır. Userid (ParentId) gönderilmesi zorunludur çünkü bebekler bir kullanıcıya bağlıdır.
/// 
/// </remarks>
/// <param name="kidId">İşlem yapılacak bebeğin ID'si</param>
/// <response code="200">Kayıt başarıyla eklendiğinde döner</response>
/// <response code="400">Bebek bulunamazsa veya veriler hatalıysa döner</response>
        [HttpPost("{userId}")]
        public async Task<IActionResult> Create(int userId, [FromBody] KidCreateDto dto)
        {
            if (dto == null)
                throw new Exception("Bebek verisi boş olamaz!");

            // HELPER: Kullanıcı var mı kontrolü
            await EnsureUserExistsAsync(userId);

            var newKid = new Kid
            {
                Name = dto.Name,
                Age = dto.Age,
                Gender = dto.Gender,
                ParentId = userId,
                Weight = dto.Weight,
                Height = dto.Height,
                GrowHistories = new List<GrowHistory>
                {
                    new GrowHistory { Weight = dto.Weight, Height = dto.Height }
                }
            };

            _dbContext.Kids.Add(newKid);
            await _dbContext.SaveChangesAsync();

            return Ok(new ApiResponse<KidResponseDto>
            {
                Message = "Bebek başarıyla oluşturuldu.",
                Data = MapToDto(newKid) // HELPER: Döngüden kaçınmak için DTO'ya çeviriyoruz
            });
        }

        // ==========================================
        // 2. READ (Bir Kullanıcının Tüm Bebeklerini Getir)
        // ==========================================
        [HttpGet("parent/{userId}")]
        public async Task<IActionResult> GetKidsByParentId(int userId)
        {
            await EnsureUserExistsAsync(userId);

            var kids = await _dbContext.Kids
                .Where(k => k.ParentId == userId)
                .ToListAsync();

            // Tüm listeyi DTO listesine çeviriyoruz
            var kidDtos = kids.Select(k => MapToDto(k)).ToList();

            return Ok(new ApiResponse<List<KidResponseDto>>
            {
                Message = "Bebekler başarıyla listelendi.",
                Data = kidDtos
            });
        }

        // ==========================================
        // 3. READ (Tek Bir Bebeği ID ile Getir)
        // ==========================================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // HELPER: Bebek var mı kontrolü ve getirme işlemi
            var kid = await GetKidByIdAsync(id);

            return Ok(new ApiResponse<KidResponseDto>
            {
                Message = "Bebek bilgisi getirildi.",
                Data = MapToDto(kid)
            });
        }

        // ==========================================
        // 4. UPDATE (Bebek Bilgilerini Güncelle)
        // ==========================================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] KidUpdateDto dto)
        {
            if (dto == null)
                throw new Exception("Güncellenecek veri boş olamaz!");

            var kid = await GetKidByIdAsync(id);

            // Yeni değerleri atıyoruz
            kid.Name = dto.Name;
            kid.Age = dto.Age;
            kid.Weight = dto.Weight;
            kid.Height = dto.Height;

            // İsteğe bağlı: Boy ve kilo değiştiyse GrowHistory'ye yeni kayıt ekleyebilirsiniz.
            // kid.GrowHistories.Add(new GrowHistory { Weight = dto.Weight, Height = dto.Height });

            _dbContext.Kids.Update(kid);
            await _dbContext.SaveChangesAsync();

            return Ok(new ApiResponse<KidResponseDto>
            {
                Message = "Bebek bilgileri başarıyla güncellendi.",
                Data = MapToDto(kid)
            });
        }

        // ==========================================
        // 5. DELETE (Bebeği Sil)
        // ==========================================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var kid = await GetKidByIdAsync(id);

            _dbContext.Kids.Remove(kid);
            await _dbContext.SaveChangesAsync();

            return Ok(new ApiResponse<object>
            {
                Message = "Bebek başarıyla silindi.",
                Data = null
            });
        }


        /// <summary>
        /// Kullanıcının (Parent) veritabanında olup olmadığını kontrol eder. Yoksa Exception fırlatır.
        /// </summary>
        private async Task EnsureUserExistsAsync(int userId)
        {
            var userExists = await _dbContext.Users.AnyAsync(u => u.Id == userId); // Modelinize göre Id veya ID olabilir
            if (!userExists)
                throw new Exception("Bağlı kullanıcı bulunamadı!");
        }

        /// <summary>
        /// ID'si verilen bebeği bulur. Bulamazsa Exception fırlatır.
        /// </summary>
        private async Task<Kid> GetKidByIdAsync(int kidId)
        {
            var kid = await _dbContext.Kids.FindAsync(kidId);
            if (kid == null)
                throw new Exception("Bebek bulunamadı!");
            
            return kid;
        }

        /// <summary>
        /// Veritabanı modelini (Kid), döngüsel başvuru içermeyen güvenli modele (KidResponseDto) dönüştürür.
        /// </summary>
        private KidResponseDto MapToDto(Kid kid)
        {
            return new KidResponseDto
            {
                ID = kid.ID,
                Name = kid.Name,
                ParentId = kid.ParentId,
                Age = kid.Age,
                Gender = kid.Gender,
                Weight = kid.Weight,
                Height = kid.Height
            };
        }
    }
}