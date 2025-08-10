using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kitapMagazaApi.Data;
using kitapMagazaApi.Models;
using kitapMagazaApi.DTOs;

namespace kitapMagazaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly kitapMagazaDbContext _context;
        
        public CategoriesController(kitapMagazaDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetCategories()
        {
            var categories = await _context.Categories
                .Select(c => new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    CreatedDate = c.CreatedDate,
                    kitapCount = c.Kitaplar.Count(b => b.IsActive)
                })
                .ToListAsync();
            
            return Ok(categories);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategory(int id)
        {
            var category = await _context.Categories
                .Where(c => c.Id == id)
                .Select(c => new CategoryResponseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    CreatedDate = c.CreatedDate,
                    kitapCount = c.Kitaplar.Count(b => b.IsActive)
                })
                .FirstOrDefaultAsync();
            
            if (category == null)
            {
                return NotFound();
            }
            
            return Ok(category);
        }
        
        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> CreateCategory(CategoryCreateDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                CreatedDate = DateTime.Now
            };
            
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            
            var responseDto = new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedDate = category.CreatedDate,
                kitapCount = 0
            };
            
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, responseDto);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryCreateDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            
            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;
            
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            
            var haskitaps = await _context.Kitaplar.AnyAsync(b => b.CategoryId == id && b.IsActive);
            if (haskitaps)
            {
                return BadRequest("Cannot delete category that has active kitaps");
            }
            
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}
