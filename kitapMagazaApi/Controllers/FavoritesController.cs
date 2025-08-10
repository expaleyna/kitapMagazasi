using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kitapMagazaApi.Data;
using kitapMagazaApi.Models;
using kitapMagazaApi.DTOs;

namespace kitapMagazaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritesController : ControllerBase
    {
        private readonly kitapMagazaDbContext _context;
        
        public FavoritesController(kitapMagazaDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FavoriteResponseDto>>> GetFavorites(int userId)
        {
            var favorites = await _context.Favorites
                .Include(f => f.Kitap)
                .Where(f => f.UserId == userId)
                .Select(f => new FavoriteResponseDto
                {
                    Id = f.Id,
                    UserId = f.UserId,
                    KitapId = f.KitapId,
                    KitapTitle = f.Kitap.Title,
                    KitapAuthor = f.Kitap.Author,
                    KitapPrice = f.Kitap.Price,
                    KitapImageUrl = f.Kitap.ImageUrl,
                    CreatedDate = f.CreatedDate
                })
                .OrderByDescending(f => f.CreatedDate)
                .ToListAsync();
            
            return Ok(favorites);
        }
        
        [HttpPost]
        public async Task<ActionResult<FavoriteResponseDto>> AddFavorite(FavoriteCreateDto favoriteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = await _context.Users.FindAsync(favoriteDto.UserId);
            if (user == null)
            {
                return BadRequest("Invalid user ID");
            }
            
            var kitap = await _context.Kitaplar.FindAsync(favoriteDto.KitapId);
            if (kitap == null || !kitap.IsActive)
            {
                return BadRequest("Invalid kitap ID");
            }
            
            var existingFavorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == favoriteDto.UserId && f.KitapId == favoriteDto.KitapId);
            
            if (existingFavorite != null)
            {
                return Conflict("kitap is already in favorites");
            }
            
            var favorite = new Favorite
            {
                UserId = favoriteDto.UserId,
                KitapId = favoriteDto.KitapId,
                CreatedDate = DateTime.Now
            };
            
            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();
            
            var responseDto = new FavoriteResponseDto
            {
                Id = favorite.Id,
                UserId = favorite.UserId,
                KitapId = favorite.KitapId,
                KitapTitle = kitap.Title,
                KitapAuthor = kitap.Author,
                KitapPrice = kitap.Price,
                KitapImageUrl = kitap.ImageUrl,
                CreatedDate = favorite.CreatedDate
            };
            
            return Created($"/api/favorites/{favorite.Id}", responseDto);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveFavorite(int id)
        {
            var favorite = await _context.Favorites.FindAsync(id);
            if (favorite == null)
            {
                return NotFound();
            }
            
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        [HttpDelete("user/{userId}/kitap/{KitapId}")]
        public async Task<IActionResult> RemoveFavoriteByUserAndkitap(int userId, int KitapId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.KitapId == KitapId);
            
            if (favorite == null)
            {
                return NotFound();
            }
            
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}
