using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kitapMagazaApi.Data;
using kitapMagazaApi.Models;
using kitapMagazaApi.DTOs;

namespace kitapMagazaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly kitapMagazaDbContext _context;

        public AdminController(kitapMagazaDbContext context)
        {
            _context = context;
        }

        // Dashboard istatistikleri
        [HttpGet("dashboard")]
        public async Task<ActionResult<object>> GetDashboardStats()
        {
            try
            {
                var totalUsers = await _context.Users.CountAsync();
                var totalkitaps = await _context.Kitaplar.CountAsync();
                var totalOrders = await _context.Orders.CountAsync();
                var totalCategories = await _context.Categories.CountAsync();
                var totalRevenue = await _context.Orders
                    .Include(o => o.OrderItems)
                    .Where(o => o.Status == "Completed")
                    .SelectMany(o => o.OrderItems)
                    .SumAsync(oi => oi.UnitPrice * oi.Quantity);

                var recentOrders = await _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Kitap)
                    .OrderByDescending(o => o.OrderDate)
                    .Take(5)
                    .Select(o => new
                    {
                        Id = o.Id,
                        UserName = o.User.FirstName + " " + o.User.LastName,
                        TotalAmount = o.TotalAmount,
                        Status = o.Status,
                        OrderDate = o.OrderDate
                    })
                    .ToListAsync();

                var lowStockkitaps = await _context.Kitaplar
                    .Where(b => b.Stock < 10)
                    .Select(b => new
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Stock = b.Stock
                    })
                    .ToListAsync();

                return Ok(new
                {
                    TotalUsers = totalUsers,
                    Totalkitaps = totalkitaps,
                    TotalOrders = totalOrders,
                    TotalCategories = totalCategories,
                    TotalRevenue = totalRevenue,
                    RecentOrders = recentOrders,
                    LowStockkitaps = lowStockkitaps
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Bir hata oluştu", Error = ex.Message });
            }
        }

        // Kitap yönetimi
        [HttpPost("kitaps")]
        public async Task<ActionResult<KitapResponseDto>> CreateKitap([FromBody] KitapCreateDto createKitapDto)
        {
            try
            {
                var kitap = new Kitap
                {
                    Title = createKitapDto.Title,
                    Author = createKitapDto.Author,
                    Description = createKitapDto.Description,
                    Price = createKitapDto.Price,
                    ImageUrl = createKitapDto.ImageUrl,
                    Stock = createKitapDto.Stock,
                    CategoryId = createKitapDto.CategoryId,
                    CreatedDate = DateTime.Now
                };

                _context.Kitaplar.Add(kitap);
                await _context.SaveChangesAsync();

                var kitapDto = new KitapResponseDto
                {
                    Id = kitap.Id,
                    Title = kitap.Title,
                    Author = kitap.Author,
                    Description = kitap.Description,
                    Price = kitap.Price,
                    ImageUrl = kitap.ImageUrl,
                    Stock = kitap.Stock,
                    CategoryId = kitap.CategoryId,
                    CategoryName = (await _context.Categories.FindAsync(kitap.CategoryId))?.Name ?? "",
                    CreatedDate = kitap.CreatedDate
                };

                return CreatedAtAction(nameof(CreateKitap), new { id = kitap.Id }, kitapDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Kitap oluşturulurken hata oluştu", Error = ex.Message });
            }
        }

        [HttpPut("kitaps/{id}")]
        public async Task<IActionResult> Updatekitap(int id, [FromBody] UpdatekitapDto updatekitapDto)
        {
            try
            {
                var kitap = await _context.Kitaplar.FindAsync(id);
                if (kitap == null)
                {
                    return NotFound(new { Message = "Kitap bulunamadı" });
                }

                kitap.Title = updatekitapDto.Title;
                kitap.Author = updatekitapDto.Author;
                kitap.Description = updatekitapDto.Description;
                kitap.Price = updatekitapDto.Price;
                kitap.ImageUrl = updatekitapDto.ImageUrl;
                kitap.Stock = updatekitapDto.Stock;
                kitap.CategoryId = updatekitapDto.CategoryId;

                await _context.SaveChangesAsync();
                return Ok(new { Message = "Kitap başarıyla güncellendi" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Kitap güncellenirken hata oluştu", Error = ex.Message });
            }
        }

        [HttpDelete("kitaps/{id}")]
        public async Task<IActionResult> Deletekitap(int id)
        {
            try
            {
                var kitap = await _context.Kitaplar.FindAsync(id);
                if (kitap == null)
                {
                    return NotFound(new { Message = "Kitap bulunamadı" });
                }

                _context.Kitaplar.Remove(kitap);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Kitap başarıyla silindi" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Kitap silinirken hata oluştu", Error = ex.Message });
            }
        }

        // Kategori yönetimi
        [HttpPost("categories")]
        public async Task<ActionResult<CategoryResponseDto>> CreateCategory([FromBody] CategoryCreateDto createCategoryDto)
        {
            try
            {
                var category = new Category
                {
                    Name = createCategoryDto.Name,
                    Description = createCategoryDto.Description,
                    CreatedDate = DateTime.Now
                };

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                var categoryDto = new CategoryResponseDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    CreatedDate = category.CreatedDate
                };

                return CreatedAtAction(nameof(CreateCategory), new { id = category.Id }, categoryDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Kategori oluşturulurken hata oluştu", Error = ex.Message });
            }
        }

        [HttpPut("categories/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryCreateDto updateCategoryDto)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound(new { Message = "Kategori bulunamadı" });
                }

                category.Name = updateCategoryDto.Name;
                category.Description = updateCategoryDto.Description;

                await _context.SaveChangesAsync();
                return Ok(new { Message = "Kategori başarıyla güncellendi" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Kategori güncellenirken hata oluştu", Error = ex.Message });
            }
        }

        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _context.Categories
                    .Include(c => c.Kitaplar)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                {
                    return NotFound(new { Message = "Kategori bulunamadı" });
                }

                if (category.Kitaplar.Any())
                {
                    return BadRequest(new { Message = "Bu kategoriye ait kitaplar bulunmaktadır. Önce kitapları silin veya başka kategoriye taşıyın." });
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Kategori başarıyla silindi" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Kategori silinirken hata oluştu", Error = ex.Message });
            }
        }

        // Kullanıcı yönetimi
        [HttpGet("users")]
        public async Task<ActionResult<List<UserResponseDto>>> GetAllUsers()
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Kullanıcılar getirilirken hata oluştu", Error = ex.Message });
            }
        }

        [HttpPut("users/{id}/role")]
        public async Task<IActionResult> UpdateUserRole(int id, [FromBody] UpdateUserRoleDto updateUserRoleDto)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { Message = "Kullanıcı bulunamadı" });
                }

                user.Role = updateUserRoleDto.Role;
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Kullanıcı rolü başarıyla güncellendi" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Kullanıcı rolü güncellenirken hata oluştu", Error = ex.Message });
            }
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound(new { Message = "Kullanıcı bulunamadı" });
                }

                if (user.Role == "Admin")
                {
                    return BadRequest(new { Message = "Admin kullanıcı silinemez" });
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Kullanıcı başarıyla silindi" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Kullanıcı silinirken hata oluştu", Error = ex.Message });
            }
        }

        // Sipariş yönetimi
        [HttpGet("orders")]
        public async Task<ActionResult<List<object>>> GetAllOrders()
        {
            try
            {
                var orders = await _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Kitap)
                    .OrderByDescending(o => o.OrderDate)
                    .Select(o => new
                    {
                        Id = o.Id,
                        UserName = o.User.FirstName + " " + o.User.LastName,
                        TotalAmount = o.TotalAmount,
                        Status = o.Status,
                        OrderDate = o.OrderDate,
                        ShippingAddress = o.ShippingAddress,
                        OrderItems = o.OrderItems.Select(oi => new
                        {
                            KitapTitle = oi.Kitap.Title,
                            Quantity = oi.Quantity,
                            UnitPrice = oi.UnitPrice
                        }).ToList()
                    })
                    .ToListAsync();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Siparişler getirilirken hata oluştu", Error = ex.Message });
            }
        }

        [HttpPut("orders/{id}/status")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] UpdateOrderStatusDto updateOrderStatusDto)
        {
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    return NotFound(new { Message = "Sipariş bulunamadı" });
                }

                order.Status = updateOrderStatusDto.Status;
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Sipariş durumu başarıyla güncellendi" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Sipariş durumu güncellenirken hata oluştu", Error = ex.Message });
            }
        }
    }
}
