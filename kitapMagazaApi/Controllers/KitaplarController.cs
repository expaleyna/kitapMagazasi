using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kitapMagazaApi.Data;
using kitapMagazaApi.Models;
using kitapMagazaApi.DTOs;

namespace kitapMagazaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KitaplarController : ControllerBase
    {
        private readonly kitapMagazaDbContext _context;
        
        public KitaplarController(kitapMagazaDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KitapResponseDto>>> GetKitaplar(
            int? categoryId = null, 
            decimal? minPrice = null, 
            decimal? maxPrice = null,
            string? search = null,
            string? sortBy = null,
            string? sortOrder = null)
        {
            var query = _context.Kitaplar
                .Include(b => b.Category)
                .Where(b => b.IsActive);
            
            if (categoryId.HasValue)
            {
                query = query.Where(b => b.CategoryId == categoryId);
            }
            
            if (minPrice.HasValue)
            {
                query = query.Where(b => b.Price >= minPrice);
            }
            
            if (maxPrice.HasValue)
            {
                query = query.Where(b => b.Price <= maxPrice);
            }
            
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b => b.Title.Contains(search) || b.Author.Contains(search));
            }
            
            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "price":
                        query = sortOrder?.ToLower() == "desc" 
                            ? query.OrderByDescending(b => b.Price)
                            : query.OrderBy(b => b.Price);
                        break;
                    case "title":
                        query = sortOrder?.ToLower() == "desc" 
                            ? query.OrderByDescending(b => b.Title)
                            : query.OrderBy(b => b.Title);
                        break;
                    case "author":
                        query = sortOrder?.ToLower() == "desc" 
                            ? query.OrderByDescending(b => b.Author)
                            : query.OrderBy(b => b.Author);
                        break;
                    default:
                        query = query.OrderBy(b => b.Id);
                        break;
                }
            }
            else
            {
                query = query.OrderBy(b => b.Id);
            }
            
            var kitapList = await query.ToListAsync();
            var kitapDtos = kitapList.Select(b => new KitapResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Description = b.Description,
                Price = b.Price,
                ImageUrl = b.ImageUrl,
                Stock = b.Stock,
                IsActive = b.IsActive,
                CategoryId = b.CategoryId,
                CategoryName = b.Category.Name,
                CreatedDate = b.CreatedDate
            }).ToList();
            
            return Ok(kitapDtos);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<KitapResponseDto>> GetKitap(int id)
        {
            var kitap = await _context.Kitaplar
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id && b.IsActive);
            
            if (kitap == null)
            {
                return NotFound();
            }
            
            var kitapDto = new KitapResponseDto
            {
                Id = kitap.Id,
                Title = kitap.Title,
                Author = kitap.Author,
                Description = kitap.Description,
                Price = kitap.Price,
                ImageUrl = kitap.ImageUrl,
                Stock = kitap.Stock,
                IsActive = kitap.IsActive,
                CategoryId = kitap.CategoryId,
                CategoryName = kitap.Category.Name,
                CreatedDate = kitap.CreatedDate
            };
            
            return Ok(kitapDto);
        }
        
        [HttpPost]
        public async Task<ActionResult<KitapResponseDto>> PostKitap(KitapCreateDto kitapDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var kitap = new Kitap
            {
                Title = kitapDto.Title,
                Author = kitapDto.Author,
                Description = kitapDto.Description,
                Price = kitapDto.Price,
                ImageUrl = kitapDto.ImageUrl,
                Stock = kitapDto.Stock,
                CategoryId = kitapDto.CategoryId,
                CreatedDate = DateTime.Now
            };
            
            _context.Kitaplar.Add(kitap);
            await _context.SaveChangesAsync();
            
            await _context.Entry(kitap)
                .Reference(b => b.Category)
                .LoadAsync();
            
            var responseDto = new KitapResponseDto
            {
                Id = kitap.Id,
                Title = kitap.Title,
                Author = kitap.Author,
                Description = kitap.Description,
                Price = kitap.Price,
                ImageUrl = kitap.ImageUrl,
                Stock = kitap.Stock,
                IsActive = kitap.IsActive,
                CategoryId = kitap.CategoryId,
                CategoryName = kitap.Category.Name,
                CreatedDate = kitap.CreatedDate
            };
            
            return CreatedAtAction(nameof(GetKitap), new { id = kitap.Id }, responseDto);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKitap(int id, KitapCreateDto kitapDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var kitap = await _context.Kitaplar.FindAsync(id);
            if (kitap == null)
            {
                return NotFound();
            }
            
            kitap.Title = kitapDto.Title;
            kitap.Author = kitapDto.Author;
            kitap.Description = kitapDto.Description;
            kitap.Price = kitapDto.Price;
            kitap.ImageUrl = kitapDto.ImageUrl;
            kitap.Stock = kitapDto.Stock;
            kitap.CategoryId = kitapDto.CategoryId;
            
            _context.Entry(kitap).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KitapExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKitap(int id)
        {
            var kitap = await _context.Kitaplar.FindAsync(id);
            if (kitap == null)
            {
                return NotFound();
            }
            
            kitap.IsActive = false;
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        private bool KitapExists(int id)
        {
            return _context.Kitaplar.Any(e => e.Id == id);
        }
    }
}
