using Microsoft.AspNetCore.Mvc;
namespace bebegim.Controller;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{

[HttpGet("test-hata")]
public IActionResult Test(int id)
{
    // Hiçbir try-catch yok!
    if (id <= 5)
    throw new Exception("Bebek verisi çekilirken bir sorun oluştu!");
    else  return Ok("Sorunsuz çalıştı!");
}

}



