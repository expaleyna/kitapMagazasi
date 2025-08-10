using System.ComponentModel.DataAnnotations;

namespace kitapMagazaApi.DTOs
{
    public class OrderCreateDto
    {
        [Required]
        public int UserId { get; set; }
        
        [StringLength(500)]
        public string? ShippingAddress { get; set; }
        
        [Required]
        public List<OrderItemCreateDto> OrderItems { get; set; } = new List<OrderItemCreateDto>();
    }
    
    public class OrderItemCreateDto
    {
        [Required]
        public int KitapId { get; set; }
        
        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
    
    public class OrderResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public string? ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemResponseDto> OrderItems { get; set; } = new List<OrderItemResponseDto>();
    }
    
    public class OrderItemResponseDto
    {
        public int Id { get; set; }
        public int KitapId { get; set; }
        public string KitapTitle { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
