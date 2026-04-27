using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bebegim.Data;
using bebegim.Models;
namespace bebegim.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly BebegimDbContext dbContext;
    private readonly ILogger<UserController> logger;


    public UserController(BebegimDbContext _context, ILogger<UserController> _logger)
    {  
        dbContext = _context;
        logger = _logger;

    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var users = await dbContext.Users.ToListAsync();
        return Ok(users);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto request)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null )
        {
            return BadRequest("Invalid email or password");
        }
        return Ok(new { message = "Login successful", user = new {
            id= user.Id,
            name= user.Name,
            surname= user.Surname,
            email= user.Email
        }});
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto request)
    {
        if (await dbContext.Users.AnyAsync(u => u.Email == request.Email))
        {
            return BadRequest("Email already in use");
        }

        var user = new User()
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            PasswordHash = request.PasswordHash
        };

        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        return Ok(new { message = "Registration successful" });
    }

    [HttpGet("add-kid-to-user")]
    public async Task<IActionResult> AddKidToUser(int userId, Kid kid)
    {
        


        dbContext.Kids.Add(kid);
        await dbContext.SaveChangesAsync();

        // Burada user-kid ilişkisi kurulacak (örneğin, bir UserKids tablosu olabilir)
        return Ok(new { message = "Kid added to user successfully" });
    }

}