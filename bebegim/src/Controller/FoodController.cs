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
    public class FoodController : ControllerBase
    {
        private readonly BebegimDbContext _context;

        public FoodController(BebegimDbContext context)
        {
            _context = context;
        }


        /// <summary>
///Custom yemek eklemek için kullanılır. 
/// </summary>



        [HttpPost("Create")]
        public async Task<IActionResult> CreateFood([FromBody] FoodCreateDto dto)
        {
            if (dto == null)
                throw new Exception("Yiyecek verisi boş olamaz!"); // Middleware yakalayacak

            var food = new Food
            {
                KidId = dto.KidId,
                Name = dto.Name, 
                Description = dto.Description,
                AddedDate = DateTime.UtcNow
            };

            _context.Foods.Add(food);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Yiyecek başarıyla eklendi.", id = food.Id });
        }

                /// <summary>
///Bebek için eklenmiş tüm yiyecekleri getirir.
/// </summary>



        [HttpGet("List/{kidId}")]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoodList(int kidId)
        {
            
            return await _context.Foods
                .Where(f => f.KidId == kidId)
                .OrderBy(f => f.Name)
                .ToListAsync();
        }

    


   
    }


}