using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kitapMagazaApi.Data;
using kitapMagazaApi.Models;
using kitapMagazaApi.DTOs;

namespace kitapMagazaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly kitapMagazaDbContext _context;
        
        public UsersController(kitapMagazaDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            var users = await _context.Users
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.Role,
                    Phone = u.Phone,
                    Address = u.Address,
                    CreatedDate = u.CreatedDate
                })
                .ToListAsync();
            
            return Ok(users);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUser(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserResponseDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Role = u.Role,
                    Phone = u.Phone,
                    Address = u.Address,
                    CreatedDate = u.CreatedDate
                })
                .FirstOrDefaultAsync();
            
            if (user == null)
            {
                return NotFound();
            }
            
            return Ok(user);
        }
        
        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> CreateUser(UserCreateDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (existingUser != null)
            {
                return BadRequest("Email already exists");
            }
            
            var user = new User
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = userDto.Password, // In production, hash this password
                Role = "User",
                Phone = userDto.Phone,
                Address = userDto.Address,
                CreatedDate = DateTime.Now
            };
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            var responseDto = new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                Phone = user.Phone,
                Address = user.Address,
                CreatedDate = user.CreatedDate
            };
            
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, responseDto);
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<UserResponseDto>> Login(UserLoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email && u.Password == loginDto.Password);
            
            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }
            
            var responseDto = new UserResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Role = user.Role,
                Phone = user.Phone,
                Address = user.Address,
                CreatedDate = user.CreatedDate
            };
            
            return Ok(responseDto);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserCreateDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userDto.Email && u.Id != id);
            if (existingUser != null)
            {
                return BadRequest("Email already exists");
            }
            
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Email = userDto.Email;
            user.Password = userDto.Password; // In production, hash this password
            user.Phone = userDto.Phone;
            user.Address = userDto.Address;
            
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            
            var hasOrders = await _context.Orders.AnyAsync(o => o.UserId == id);
            if (hasOrders)
            {
                return BadRequest("Cannot delete user with existing orders");
            }
            
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}
