using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using kitapMagazaApi.Data;
using kitapMagazaApi.Models;
using kitapMagazaApi.DTOs;

namespace kitapMagazaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly kitapMagazaDbContext _context;
        
        public OrdersController(kitapMagazaDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponseDto>>> GetOrders(int? userId = null)
        {
            var query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Kitap)
                .AsQueryable();
            
            if (userId.HasValue)
            {
                query = query.Where(o => o.UserId == userId);
            }
            
            var orders = await query
                .Select(o => new OrderResponseDto
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    UserName = o.User.FirstName + " " + o.User.LastName,
                    CreatedDate = o.CreatedDate,
                    ShippingAddress = o.ShippingAddress,
                    TotalAmount = o.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice),
                    OrderItems = o.OrderItems.Select(oi => new OrderItemResponseDto
                    {
                        Id = oi.Id,
                        KitapId = oi.KitapId,
                        KitapTitle = oi.Kitap.Title,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice
                    }).ToList()
                })
                .OrderByDescending(o => o.CreatedDate)
                .ToListAsync();
            
            return Ok(orders);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Kitap)
                .Where(o => o.Id == id)
                .Select(o => new OrderResponseDto
                {
                    Id = o.Id,
                    UserId = o.UserId,
                    UserName = o.User.FirstName + " " + o.User.LastName,
                    CreatedDate = o.CreatedDate,
                    ShippingAddress = o.ShippingAddress,
                    TotalAmount = o.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice),
                    OrderItems = o.OrderItems.Select(oi => new OrderItemResponseDto
                    {
                        Id = oi.Id,
                        KitapId = oi.KitapId,
                        KitapTitle = oi.Kitap.Title,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice
                    }).ToList()
                })
                .FirstOrDefaultAsync();
            
            if (order == null)
            {
                return NotFound();
            }
            
            return Ok(order);
        }
        
        [HttpPost]
        public async Task<ActionResult<OrderResponseDto>> CreateOrder(OrderCreateDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var user = await _context.Users.FindAsync(orderDto.UserId);
            if (user == null)
            {
                return BadRequest("Invalid user ID");
            }
            
            if (!orderDto.OrderItems.Any())
            {
                return BadRequest("Order must contain at least one item");
            }
            
            var order = new Order
            {
                UserId = orderDto.UserId,
                ShippingAddress = orderDto.ShippingAddress,
                CreatedDate = DateTime.Now
            };
            
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            
            // Add order items
            foreach (var itemDto in orderDto.OrderItems)
            {
                var kitap = await _context.Kitaplar.FindAsync(itemDto.KitapId);
                if (kitap == null || !kitap.IsActive)
                {
                    return BadRequest($"Invalid kitap ID: {itemDto.KitapId}");
                }
                
                if (kitap.Stock < itemDto.Quantity)
                {
                    return BadRequest($"Insufficient stock for kitap: {kitap.Title}");
                }
                
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    KitapId = itemDto.KitapId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = kitap.Price
                };
                
                _context.OrderItems.Add(orderItem);
                
                // Update stock
                kitap.Stock -= itemDto.Quantity;
            }
            
            await _context.SaveChangesAsync();
            
            // Return the created order
            return await GetOrder(order.Id);
        }
    }
}
